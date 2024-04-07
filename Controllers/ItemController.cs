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

        public ItemController(IITemRepository itemRepository) 
        {
            this.itemRepository = itemRepository;
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
                string uniqueFileName = UploadedFile(model);

                Item store = new Item
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = uniqueFileName
                };


                itemRepository.AddItem(store);

                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        private string UploadedFile(ItemViewModel model)
        {
            string uniqueFileName = null;

            if (model.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ItemImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ItemImage.CopyTo(fileStream);
                }
            }


            return uniqueFileName;
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
