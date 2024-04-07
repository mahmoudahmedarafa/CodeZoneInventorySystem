using System.ComponentModel.DataAnnotations;

namespace CodeZoneInventorySystem.ViewModels
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "Please enter item name")]
        [Display(Name = "Item Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter item description")]
        [Display(Name = "Description Name")]
        public string? Description { get; set; }

        public IFormFile ItemImage { get; set; }
    }
}
