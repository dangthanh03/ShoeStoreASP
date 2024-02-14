using System.ComponentModel.DataAnnotations;

namespace ShoeStoreASP.Models.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
