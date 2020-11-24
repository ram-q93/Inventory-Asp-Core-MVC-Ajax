using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Prompt = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Prompt = "Password")]
        public string Password { get; set; }

        [Display(Prompt = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
