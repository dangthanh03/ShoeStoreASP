using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public InvoiceController(IInvoiceService invoiceService, UserManager<ApplicationUser> userManager)
        {
                _invoiceService = invoiceService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Detail(int invoiceId)
        {
            
            // Kiểm tra invoiceId và lấy dữ liệu của hoá đơn từ cơ sở dữ liệu
            var invoiceVm = await _invoiceService.GetInvoiceAsync(invoiceId);

            // Kiểm tra dữ liệu hoá đơn
            if (invoiceVm == null)
            {
                // Nếu không tìm thấy hoá đơn, xử lý tương ứng (ví dụ: hiển thị thông báo lỗi)
                return RedirectToAction("AddToCart", "Cart"); // hoặc trang lỗi khác nếu cần
            }

            // Trả về view Detail và truyền dữ liệu hoá đơn
            return View(invoiceVm.Data);
        }
        public async Task<IActionResult> GetAllInvoice()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                string userId = user.Id.ToString();
                // Gọi service để lấy danh sách các hoá đơn của người dùng
                var result = await _invoiceService.GetAllInvoiceAsync(userId);

                // Kiểm tra kết quả trả về từ service
                if (result.IsSuccess)
                {
                    // Trả về view chứa danh sách các hoá đơn
                    return View(result.Data);
                }
                else
                {
                    // Xử lý trường hợp không thành công, có thể chuyển hướng đến trang lỗi
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return RedirectToAction("Index", "Home"); // hoặc trang lỗi khác nếu cần
            }
        }

    }
}
