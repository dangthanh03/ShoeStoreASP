using ShoeStoreASP.Models.Domain;

namespace ShoeStoreASP.Models.ViewModel
{
    public class BrandListVm
    {
        public List<Brand> Brands{ get; set; }

        // Các thuộc tính khác nếu cần

        public BrandListVm()
        {
            Brands = new List<Brand>();
        }
    }
}
