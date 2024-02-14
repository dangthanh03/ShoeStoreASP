using Microsoft.AspNetCore.Identity;

namespace ShoeStoreASP.Models.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public string Name {  get; set; }
    }
}
