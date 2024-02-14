namespace ShoeStoreASP.Models.Domain
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public ICollection<Shoe>? Shoes { get; set; }
    }
}
