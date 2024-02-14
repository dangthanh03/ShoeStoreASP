namespace ShoeStoreASP.Models.Domain
{
    public class ShoeType
    {
        public int Id { get; set; }
        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
}
