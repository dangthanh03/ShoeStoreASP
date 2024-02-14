using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;

namespace ShoeStoreASP.Service.Abstract
{
    public interface ICartService
    {
        Task<Result<string>> CreateCartAsync(string userId);
        Task<Result<string>> AddShoeToCartAsync(int cartId, int shoeId);
        Task<Result<int>> GetCartAsync(string userId);
        Task<Result<ShoeListViewModel>> GetProductAsync(int cartId);
        Task<Result<string>> RemoveFromCart(int ProductId, string userId);
        Task<Result<int>> BuyAsync(string userId);

    }
}
