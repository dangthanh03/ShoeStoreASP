using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;

namespace ShoeStoreASP.Service.Abstract
{
    public interface IInvoiceService
    {
        public Task<Result<Invoice>> CreateInvoiceAsync(Invoice invoice, List<int> productIds);
        public Task<Result<InvoiceVm>> GetInvoiceAsync(int invoiceId);
        Task<Result<List<InvoiceVm>>> GetAllInvoiceAsync(string userId);
    }
}
