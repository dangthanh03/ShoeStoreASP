using ShoeStoreASP.Models.Domain;

namespace ShoeStoreASP.Models.ViewModel
{
    public class InvoiceVm
    {
        public int? InvoiceId {  get; set; }
        public  List<Shoe> Shoes { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
