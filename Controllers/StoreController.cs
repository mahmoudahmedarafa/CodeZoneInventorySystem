using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Repositories;
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
            Store item = storeRepository.GetStore(id);

            StoreEditViewModel itemEditViewModel = new StoreEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Address = item.Address,
                ExistingPhotoPath = item.Image
            };


            return View(itemEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StoreEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Store store = storeRepository.GetStore(model.Id);
                // Update the employee object with the data in the model object
                Store updatedStore = new Store()
                {
                    Name = model.Name,
                    Address = model.Address
                };

                //Store currentStore = context.Stores.Where(s => s.Id == model.CurrentStoreId)
                //                                  .FirstOrDefault();

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.StoreImage != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    store.Image = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                storeRepository.EditStore(model.Id, updatedStore);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string ProcessUploadedFile(StoreViewModel model)
        {
            string uniqueFileName = null;

            if (model.StoreImage != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.StoreImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.StoreImage.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Delete(int storeId)
        {
            storeRepository.DeleteStore(storeId);

            return RedirectToAction("index");
        }
    }
}
