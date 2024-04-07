using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Services;
using CodeZoneInventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CodeZoneInventorySystem.Controllers
{
    public class StockController : Controller
    {
        private readonly AppDbContext context;

        public StockController(AppDbContext context) 
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult IncreaseStock()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IncreaseStock(IFormCollection formcollection)
        {
            StockViewModel stock = new StockViewModel();
            stock.SelectedStore = Convert.ToInt32(formcollection["selectedStore"]);
            stock.SelectedItem = Convert.ToInt32(formcollection["selectedItem"]);
            stock.Quantity = Convert.ToInt32(formcollection["quantity"]);

            JsonResponseViewModel model = new JsonResponseViewModel();
            //MAKE DB CALL and handle the response
            if (stock != null)
            {
                var storeItem = context.StoreItems
                                    .Where(st => st.StoreId == stock.SelectedStore && st.ItemId == stock.SelectedItem)
                                    .FirstOrDefault();

                if (storeItem != null)
                {
                    int quantity = stock.Quantity;

                    if (quantity > 0)
                    {
                        storeItem.Quantity += quantity;

                        context.SaveChanges();

                        ViewBag.Quantity = storeItem.Quantity;

                        model.ResponseCode = 0;
                        model.ResponseMessage = JsonConvert.SerializeObject(storeItem.Quantity);
                    }
                    else
                    {
                        model.ResponseCode = 1;
                        model.ResponseMessage = "Quantity is not a positive number.";
                    }
                }
            }
            else
            {
                model.ResponseCode = 1;
                model.ResponseMessage = "No items found in this store.";
            }

            return Json(model);
        }
    }
}
