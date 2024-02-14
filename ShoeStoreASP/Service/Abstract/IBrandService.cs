using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;

namespace ShoeStoreASP.Service.Abstract
{
    public interface IBrandService
    {
        Result<BrandListVm> GetAllBrand();
        Result<Brand> Add(Brand brand);
        Result<Brand> GetBrandById(int brandId);
        Result<Brand> UpdateBrand(Brand brand);
        Result<Brand> DeleteBrand(int brandId);

    }
}
