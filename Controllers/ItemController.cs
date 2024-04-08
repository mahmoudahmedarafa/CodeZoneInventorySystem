using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Repositories;
using CodeZoneInventorySystem.Services;
using CodeZoneInventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CodeZoneInventorySystem.Controllers
{
    public class ItemController : Controller
    {
        private readonly IITemRepository itemRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ItemController(IITemRepository itemRepository, IWebHostEnvironment hostingEnvironment) 
        {
            this.itemRepository = itemRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var allItems = itemRepository.GetAllItems();

            return View(allItems);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.ItemImage != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ItemImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.ItemImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Item newItem = new Item
                {
                    Name = model.Name,
                    Description = model.Description,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    Image = uniqueFileName
                };

                itemRepository.AddItem(newItem);

                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Item item = itemRepository.GetItem(id);


            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Item model)
        {

            if (ModelState.IsValid)
            {

                Item item = itemRepository.GetItem(model.Id);
                Item updatedItem = new Item();

                updatedItem.Name = model.Name;
                updatedItem.Description = model.Description;

                itemRepository.EditItem(item.Id, updatedItem);

                return RedirectToAction("index");
            }

            return View(model);
        }

        public IActionResult Delete(int itemId)
        {
            itemRepository.DeleteItem(itemId);

            return RedirectToAction("index");
        }
    }
}
