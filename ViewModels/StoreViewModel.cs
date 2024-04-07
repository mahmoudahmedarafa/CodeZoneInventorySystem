using System.ComponentModel.DataAnnotations;

namespace CodeZoneInventorySystem.ViewModels
{
    public class StoreViewModel
    {
        [Required(ErrorMessage = "Please enter store name")]
        [Display(Name = "Store Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter store address")]
        [Display(Name = "Store Address")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Please choose store image")]
        [Display(Name = "Store Image")]
        public IFormFile StoreImage { get; set; }
    }
}
