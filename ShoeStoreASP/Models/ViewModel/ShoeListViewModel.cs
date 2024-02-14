using ShoeStoreASP.Models.Domain;

namespace ShoeStoreASP.Models.ViewModel
{
    public class ShoeListViewModel
    {
        public string selectedTypes { get; set; }
        public List<int>? Types { get; set; }
        public int? BrandId { get; set; }

        public List<Shoe> Shoes { get; set; }
        public List<ShoeStoreASP.Models.Domain.Type> AllTypes { get; set; }
        public List<Brand> AllBrands{ get; set; }

        // Các thuộc tính khác nếu cần
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
        public ShoeListViewModel()
        {
            Shoes = new List<Shoe>();
        }
    }
}
