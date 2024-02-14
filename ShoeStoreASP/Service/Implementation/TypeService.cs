using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Service.Implementation
{
    public class TypeService: ITypeService
    {
        private readonly DatabaseContext _context;
        public TypeService(DatabaseContext context)
        {
            _context = context;
        }
        public Result<ShoeStoreASP.Models.Domain.Type> AddType(ShoeStoreASP.Models.Domain.Type type)
        {
            try
            {
                // Kiểm tra xem Type đã tồn tại chưa
                if (_context.Types.Any(t => t.TypeName == type.TypeName))
                {
                    return Result<ShoeStoreASP.Models.Domain.Type>.Fail("Type with the same name already exists.");
                }

                _context.Types.Add(type);
                _context.SaveChanges();

                return Result<ShoeStoreASP.Models.Domain.Type>.Success(type);
            }
            catch (Exception ex)
            {
                return Result<ShoeStoreASP.Models.Domain.Type>.Fail($"An error occurred while adding Type: {ex.Message}");
            }
        }
        public Result<TypeListVm> GetAllTypes()
        {
            try
            {
                var Types = _context.Types.ToList();
                var TypeVm = new TypeListVm { Types = Types};
                return Result<TypeListVm>.Success(TypeVm);
            }
            catch (Exception e)
            {
                return Result<TypeListVm>.Fail("An error occurred while fetching types.");
            }

        }


        public Result<ShoeStoreASP.Models.Domain.Type> UpdateType(ShoeStoreASP.Models.Domain.Type type)
        {
            try
            {
                var existingType = _context.Types.Find(type.TypeId);

                if (existingType == null)
                {
                    return Result<ShoeStoreASP.Models.Domain.Type>.Fail("An error occurred while fetching types.");
                }

                existingType.TypeName = type.TypeName;
                existingType.Description = type.Description;

                _context.SaveChanges();

                return Result<ShoeStoreASP.Models.Domain.Type>.Success(existingType);
            }
            catch (Exception ex)
            {
                return Result<ShoeStoreASP.Models.Domain.Type>.Fail("An error occurred while fetching types.");
            }
        }

        public Result<ShoeStoreASP.Models.Domain.Type> DeleteType(int typeId)
        {
            try
            {
                var type = _context.Types.Find(typeId);

                if (type == null)
                {
                    return Result<ShoeStoreASP.Models.Domain.Type>.Fail("Type not found");
                }

                _context.Types.Remove(type);
                _context.SaveChanges();

                return Result <ShoeStoreASP.Models.Domain.Type>.Success(new ShoeStoreASP.Models.Domain.Type());
            }
            catch (Exception ex)
            {
                return Result<ShoeStoreASP.Models.Domain.Type>.Fail($"An error occurred while deleting type: {ex.Message}");
            }
        }

        public Result<ShoeStoreASP.Models.Domain.Type> GetTypeById(int typeId)
        {
            try
            {
                var type = _context.Types.Find(typeId);

                if (type == null)
                {
                    return Result<ShoeStoreASP.Models.Domain.Type>.Fail($"Type with ID {typeId} not found.");
                }

                return Result<ShoeStoreASP.Models.Domain.Type>.Success(type);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về Result với thông báo lỗi nếu cần
                return Result<ShoeStoreASP.Models.Domain.Type>.Fail($"An error occurred while fetching type: {ex.Message}");
            }
        }






    }
}
