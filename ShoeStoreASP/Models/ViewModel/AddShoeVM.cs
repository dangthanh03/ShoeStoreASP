using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreASP.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreASP.Models.ViewModel
{
    public class AddShoeVM
    {
        public string Name { get; set; }
        public int? BrandId { get; set; }
        public List<Brand> Brands { get; set; }
        public List<int> Types { get; set; }
        public List<ShoeStoreASP.Models.Domain.Type> AllTypes { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string[] TypesString { get; set; }

    }
}
