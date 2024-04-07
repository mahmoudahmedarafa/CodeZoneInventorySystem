using System.ComponentModel.DataAnnotations;

namespace CodeZoneInventorySystem.ViewModels
{
    public class StoreViewModel
    {
        [Required(ErrorMessage = "Please enter store name")]
        [Display(Name = "Store Name")]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter store address")]
        [Display(Name = "Store Address")]
        [MaxLength(100)]
        public string Address { get; set; } = null!;
        public IFormFile StoreImage { get; set; }
    }
}
