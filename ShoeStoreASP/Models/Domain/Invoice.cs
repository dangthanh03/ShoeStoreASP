namespace ShoeStoreASP.Models.Domain
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string UserId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
