using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Prompt = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Prompt = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email! (example@example.org)")]
        [Display(Prompt = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Prompt = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Prompt = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Prompt = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
