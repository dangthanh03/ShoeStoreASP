using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Service.Implementation
{
    public class InvoiceService: IInvoiceService
    {
        private readonly DatabaseContext _context;
        public InvoiceService(DatabaseContext context)
        {

            _context = context;

        }
        public async Task<Result<Invoice>> CreateInvoiceAsync(Invoice invoice, List<int> productIds)
        {
            try
            {
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                foreach (var id in productIds)
                {
                    var invoiceShoe = new InvoiceShoe
                    {
                        ShoeId = id,
                        InvoiceId = invoice.InvoiceId

                };
                    _context.InvoiceShoes.Add(invoiceShoe);
                    await _context.SaveChangesAsync();
                }
                
                return Result<Invoice>.Success(invoice);
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Result<Invoice>.Fail("Failed to create invoice: " + ex.Message);
            }
        }
        public async Task<Result<InvoiceVm>> GetInvoiceAsync(int invoiceId)
        {
            try
            {
                // Tìm hóa đơn theo userId từ cơ sở dữ liệu
                var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
               

                if (invoice == null)
                {
                    return Result<InvoiceVm>.Fail("Invoice not found for the specified user.");
                }
                var shoeId = (from Currentinvoice in _context.Invoices
                              join invoicShoe in _context.InvoiceShoes
                              on Currentinvoice.InvoiceId equals invoicShoe.InvoiceId
                              where Currentinvoice.InvoiceId == invoice.InvoiceId
                              select invoicShoe.ShoeId).ToList();
                var shoes = (from s in _context.Shoes
                             where shoeId.Contains(s.ShoeId)
                             select s).ToList();
                var ShoeList = new InvoiceVm { 
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                Shoes = shoes
                };
                return Result<InvoiceVm>.Success(ShoeList);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Result<InvoiceVm>.Fail("Failed to fetch invoice: " + ex.Message);
            }
        }

        public async Task<Result<List<InvoiceVm>>> GetAllInvoiceAsync(string userId)
        {
            try
            {
                // Tìm các hoá đơn của người dùng từ cơ sở dữ liệu
                var invoices = await _context.Invoices
                    .Where(i => i.UserId == userId)
                    .ToListAsync();

                // Kiểm tra xem có hoá đơn nào không
                if (invoices == null || !invoices.Any())
                {
                    return Result<List<InvoiceVm>>.Fail("No invoices found for the specified user.");
                }

                // Lặp qua từng hoá đơn và lấy thông tin chi tiết
                var invoiceList = new List<InvoiceVm>();
                foreach (var invoice in invoices)
                {
                    var shoeIdList = await _context.InvoiceShoes
                        .Where(invoiceShoes => invoiceShoes.InvoiceId == invoice.InvoiceId)
                        .Select(invoiceShoes => invoiceShoes.ShoeId)
                        .ToListAsync();

                    var shoes = await _context.Shoes
                        .Where(s => shoeIdList.Contains(s.ShoeId))
                        .ToListAsync();

                    var invoiceVm = new InvoiceVm
                    {
                        InvoiceId= invoice.InvoiceId,
                        InvoiceDate = invoice.InvoiceDate,
                        TotalAmount = invoice.TotalAmount,
                        Shoes = shoes
                    };

                    invoiceList.Add(invoiceVm);
                }

                return Result<List<InvoiceVm>>.Success(invoiceList);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Result<List<InvoiceVm>>.Fail("Failed to fetch invoices: " + ex.Message);
            }
        }


    }
}
