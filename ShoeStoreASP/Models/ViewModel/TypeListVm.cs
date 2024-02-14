using ShoeStoreASP.Models.Domain;

namespace ShoeStoreASP.Models.ViewModel
{
    public class TypeListVm
    {
        public List<ShoeStoreASP.Models.Domain.Type> Types{ get; set; }

        // Các thuộc tính khác nếu cần

        public TypeListVm()
        {
            Types = new List<ShoeStoreASP.Models.Domain.Type>();
        }
    }
}
