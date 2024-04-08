using CodeZoneInventorySystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeZoneInventorySystem.ViewModels
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "Please enter item name")]
        [Display(Name = "Item Name")]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter item description")]
        [Display(Name = "Item Description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please choose item current store")]
        [Display(Name = "Item Store")]
        public int CurrentStoreId { get; set; }

        public IFormFile ItemImage { get; set; }
    }
}
