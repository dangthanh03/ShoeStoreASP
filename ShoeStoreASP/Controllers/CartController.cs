using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Controllers
{
    public class CartController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(UserManager<ApplicationUser> userManager, ICartService cartService, IInvoiceService invoiceService)
        {
            _cartService = cartService;
            _userManager = userManager;

            _invoiceService = invoiceService;

        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int shoeId)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();
            var cart = await _cartService.GetCartAsync(userId);
            var result = await _cartService.AddShoeToCartAsync(cart.Data, shoeId);
            var cartProducts = await _cartService.GetProductAsync(cart.Data);
            return View(cartProducts.Data);
        }

        public async Task<IActionResult> AddToCart()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();
            var cart = await _cartService.GetCartAsync(userId);
            var result = await _cartService.GetProductAsync(cart.Data);

            return View(result.Data);
        }
        public async Task<IActionResult> RemoveFromCart(int ProductId)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();

            var result = await _cartService.RemoveFromCart(ProductId, userId);

            return RedirectToAction(nameof(AddToCart));
        }

        public async Task<IActionResult> Buy()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();

            var result = await _cartService.BuyAsync(userId);
            if (result.IsSuccess)
            {

                return RedirectToAction("Detail", "Invoice", new { invoiceId = result.Data });

            }
            else
            {
                return RedirectToAction(nameof(AddToCart));
            }


        }



    }
}

