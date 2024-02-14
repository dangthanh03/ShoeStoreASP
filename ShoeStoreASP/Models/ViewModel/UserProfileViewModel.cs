using System.ComponentModel.DataAnnotations;

namespace ShoeStoreASP.Models.ViewModel
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
