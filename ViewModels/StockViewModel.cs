using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeZoneInventorySystem.ViewModels
{
    public class StockViewModel
    {
        public int SelectedStore { get; set; }
        public int SelectedItem { get; set; }
        public int Quantity { get; set; }
    }
}
