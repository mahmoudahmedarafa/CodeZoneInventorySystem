using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Services;
using CodeZoneInventorySystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace CodeZoneInventorySystem.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreRepository storeRepository;

        public StoreController(IStoreRepository storeRepository) 
        {
            this.storeRepository = storeRepository;
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
                string uniqueFileName = UploadedFile(model);

                Store store = new Store
                {
                    Name = model.Name,
                    Address = model.Address,
                    Image = uniqueFileName
                };


                storeRepository.AddStore(store);

                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        private string UploadedFile(StoreViewModel model)
        {
            string uniqueFileName = null;

            if (model.StoreImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.StoreImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.StoreImage.CopyTo(fileStream);
                }
            }


            return uniqueFileName;
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
