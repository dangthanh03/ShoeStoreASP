using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;

namespace ShoeStoreASP.Service.Abstract
{
    public interface ITypeService
    {
        public Result<TypeListVm> GetAllTypes();
        Result<ShoeStoreASP.Models.Domain.Type> UpdateType(ShoeStoreASP.Models.Domain.Type type);
        Result<ShoeStoreASP.Models.Domain.Type> DeleteType(int typeId);
        Result<ShoeStoreASP.Models.Domain.Type> GetTypeById(int typeId);
        Result<ShoeStoreASP.Models.Domain.Type> AddType(ShoeStoreASP.Models.Domain.Type type);
    }
}
