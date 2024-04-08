using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Services;
using CodeZoneInventorySystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeZoneInventorySystem.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreRepository storeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StoreController(IStoreRepository storeRepository, IWebHostEnvironment hostingEnvironment) 
        {
            this.storeRepository = storeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var allStores = storeRepository.GetAllStores();

            return View(allStores);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.StoreImage != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.StoreImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.StoreImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Store newStore = new Store
                {
                    Name = model.Name,
                    Address = model.Address,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    Image = uniqueFileName
                };

                storeRepository.AddStore(newStore);

                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Store store = storeRepository.GetStore(id);

            
            return View(store);
        }

        [HttpPost]
        public IActionResult Edit(Store model)
        {

            if (ModelState.IsValid)
            {

                Store store = storeRepository.GetStore(model.Id);
                Store updatedStore = new Store();

                updatedStore.Name = model.Name;
                updatedStore.Address = model.Address;

                storeRepository.EditStore(store.Id, updatedStore);

                return RedirectToAction("index");
            }

            return View(model);
        }

        public IActionResult Delete(int storeId)
        {
            storeRepository.DeleteStore(storeId);

            return RedirectToAction("index");
        }
    }
}
