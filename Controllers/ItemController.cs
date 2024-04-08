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
        private readonly AppDbContext context;

        public ItemController(IITemRepository itemRepository, IWebHostEnvironment hostingEnvironment, AppDbContext context) 
        {
            this.itemRepository = itemRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.context = context;
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

                Store currentStore = context.Stores.Where(s => s.Id == model.CurrentStoreId)
                                                   .FirstOrDefault();



                Item newItem = new Item
                {
                    Name = model.Name,
                    Description = model.Description,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    Image = uniqueFileName
                };

                itemRepository.AddItem(newItem);

                context.StoreItems.Add(new StoreItem
                {
                    StoreId = currentStore.Id,
                    ItemId = newItem.Id,
                    Quantity = 1,
                    DateReceived = DateTime.Now,
                    Item = newItem,
                    Store = currentStore
                });

                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Item item = itemRepository.GetItem(id);

            ItemEditViewModel itemEditViewModel = new ItemEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ExistingPhotoPath = item.Image
            };


            return View(itemEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ItemEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Item item = itemRepository.GetItem(model.Id);
                // Update the employee object with the data in the model object
                Item updatedItem = new Item()
                {
                    Name = model.Name,
                    Description = model.Description
                };

                //Store currentStore = context.Stores.Where(s => s.Id == model.CurrentStoreId)
                //                                  .FirstOrDefault();

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.ItemImage != null)
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
                    item.Image = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                itemRepository.EditItem(model.Id, updatedItem);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string ProcessUploadedFile(ItemViewModel model)
        {
            string uniqueFileName = null;

            if (model.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ItemImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ItemImage.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Delete(int itemId)
        {
            itemRepository.DeleteItem(itemId);

            return RedirectToAction("index");
        }
    }
}
