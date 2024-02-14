using ShoeStoreASP.Models.Domain;
using System.Net.NetworkInformation;
using ShoeStoreASP.Models.ViewModel;
namespace ShoeStoreASP.Service.Abstract
{
    public interface IUserAuthenticate
    {
        Task<Result<string>> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Result<string>> RegisterAsync(RegistrationModel model);
        Task<List<string>> GetAvailableRoles();
    }
}
