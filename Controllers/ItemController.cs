using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Repositories;
using CodeZoneInventorySystem.Services;
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
        public IActionResult Add(Item model)
        {
            if (ModelState.IsValid)
            {
                itemRepository.AddItem(model);
                ViewBag.Message = "Adding new item done.";
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
