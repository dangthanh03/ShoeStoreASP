using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreASP.Models.Domain
{
    public class Shoe
    {

        public int ShoeId { get; set; }
        public string Name { get; set; }
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        [NotMapped]
        public string? BrandName{ get; set; }
        [NotMapped]
        public string? Types{ get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


    }
}
