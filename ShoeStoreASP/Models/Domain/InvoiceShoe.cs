namespace ShoeStoreASP.Models.Domain
{
    public class InvoiceShoe
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice invoice { get; set; }
        public int ShoeId { get; set; } 
        public Shoe Shoe { get; set;}
    }
}
