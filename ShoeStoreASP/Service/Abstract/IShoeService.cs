using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;

namespace ShoeStoreASP.Service.Abstract
{
    public interface IShoeService
    {
        Result<ShoeListViewModel> GetAllShoes(string term = "", bool paging = false, int currentPage = 0, List<int> selectedTypes = null, int BrandId= -1);
        Result<string> DeleteShoe(int shoeId);
         Result<int> AddShoe(AddShoeVM model);
        Result<Shoe> GetShoeById(int ShoeId);
        Result<string>  EditShoe(EditShoeVm model);
        Result<string> UpdateImg(IFormFile formFile , int id);


    }
}
