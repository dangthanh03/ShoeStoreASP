namespace ShoeStoreASP.Models.Domain
{
    public class CartShoe
    {
        public int id { get; set; } 
        public int CartId { get; set; }
        public Cart cart;
        public int ShoeId { get; set; }
        public Shoe shoe;

    }
}
