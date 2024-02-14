using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _context;
        private readonly IInvoiceService _invoiceService;
        private readonly IShoeService _shoeService;

        public CartService(DatabaseContext databaseContext, IInvoiceService invoiceService, IShoeService shoeService)
        {
            _context = databaseContext;
            _invoiceService = invoiceService;
            _shoeService = shoeService; 
        }

        public async Task<Result<string>> AddShoeToCartAsync(int cartId, int shoeId)
        {
            try
            {
                // Kiểm tra xem có bản ghi CartShoe nào tương ứng chưa
                var existingCartShoe = await _context.CartShoes
                    .FirstOrDefaultAsync(cs => cs.CartId == cartId && cs.ShoeId == shoeId);

                if (existingCartShoe != null)
                {
                    // Nếu đã có, tăng số lượng hoặc thực hiện hành động khác tùy thuộc vào yêu cầu
                    // Ví dụ: existingCartShoe.Quantity += 1;
                    // Hoặc thực hiện hành động khác tùy thuộc vào logic kinh doanh của bạn
                }
                else
                {
                    // Nếu chưa có, tạo một bản ghi mới
                    CartShoe newCartShoe = new CartShoe
                    {
                        CartId = cartId,
                        ShoeId = shoeId
                        // Các thuộc tính khác của CartShoe nếu có
                    };

                    _context.CartShoes.Add(newCartShoe);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return Result<string>.Success("Shoe added to cart successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Error adding shoe to cart: {ex.Message}");
            }

        }

        public async Task<Result<string>> CreateCartAsync(string userId)
        {
            try
            {
                Cart cart = new Cart
                {
                    UserId = userId,
                    // Các thuộc tính khác của giỏ hàng nếu có
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                return Result<string>.Success("Cart created successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Error creating cart: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetCartAsync(string userId)
        {
            try
            {
                var result = _context.Carts.FirstOrDefault(c => c.UserId == userId);

                return Result<int>.Success(result.CartId);

            }
            catch (Exception ex)
            {
                return Result<int>.Fail("No cart exists: " + ex.Message);
            }
        }

        public async Task<Result<ShoeListViewModel>> GetProductAsync(int cartId)
        {
            try
            {
                var products = (from cart in _context.Carts
                                join cartShoe in _context.CartShoes
                                on cart.CartId equals cartShoe.CartId
                                join shoe in _context.Shoes on cartShoe.ShoeId equals shoe.ShoeId
                                where cart.CartId == cartId
                                select shoe
                               );
                var shoeList = new List<Shoe>();
                foreach (var product in products)
                {
                    shoeList.Add(product);
                }
                var shoelistVm = new ShoeListViewModel
                {
                    Shoes = shoeList
                };

                return Result<ShoeListViewModel>.Success(shoelistVm);
            }
            catch (Exception ex)
            {
                return Result<ShoeListViewModel>.Fail("Fail to get product via cart id : " + ex.Message);


            }
        }
        public async Task<Result<string>> RemoveFromCart(int productId, string userId)
        {
            try
            {
                var cart = await GetCartAsync(userId);
                if (cart.IsSuccess)
                {

                    var result = (from cartShoe in _context.CartShoes

                                  where cartShoe.CartId == cart.Data && cartShoe.ShoeId == productId
                                  select cartShoe).FirstOrDefault();
                    if (result != null)
                    {
                        _context.CartShoes.Remove(result); // Xóa bản ghi từ bộ nhớ của Entity Framework
                        await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                    }
                }

                else
                {
                    return Result<string>.Fail("Cart does not exist");

                }
                return Result<string>.Success("Product removed from cart successfully.");
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra trong quá trình xóa sản phẩm khỏi giỏ hàng
                // Bạn có thể ghi log và trả về kết quả thất bại cùng với thông báo lỗi
                return Result<string>.Fail("An error occurred while removing product from cart.");
            }
        }
        public async Task<Result<int>> BuyAsync(string userId)
        {
            try
            {
                // Get cart items by user ID
                var cart = await GetCartAsync(userId);

                // Ensure cart exists
                if (!cart.IsSuccess || cart.Data == null)
                {
                    return Result<int>.Fail("Cart not found.");
                }
                var productIds = await GetProductIdsFromCartShoes(cart.Data);
                // Ensure cart exists
                if (!productIds.IsSuccess || productIds.Data == null)
                {
                    return Result<int>.Fail("Cart is empty");
                }
                var totalAmount = CalculateTotalAmountAsync(productIds.Data);
                // Create invoice
                var invoice = new Invoice
                {
                    UserId = userId,
                    InvoiceDate = DateTime.UtcNow,
                    // Calculate total amount based on cart items or product prices
                    TotalAmount = totalAmount.Result.Data
                };

                // Save invoice to database
                var invoiceResult = await _invoiceService.CreateInvoiceAsync(invoice,productIds.Data);
                if (!invoiceResult.IsSuccess)
                {
                    return Result<int>.Fail("Failed to create invoice.");
                }

                // Clear cart after successful purchase
                var clearCartResult = await ClearCartAsync(cart.Data);
                if (!clearCartResult.IsSuccess)
                {
                    return Result<int>.Fail("Failed to clear cart after purchase.");
                }

                return Result<int>.Success(invoiceResult.Data.InvoiceId);
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Result<int>.Fail("Failed to complete purchase: " + ex.Message);
            }
        }

        private async Task<Result<decimal>> CalculateTotalAmountAsync(List<int> productIds)
        {
            try
            {
                decimal totalPrice = 0;
                foreach (var productId in productIds)
                {
                    var productResult =  _shoeService.GetShoeById(productId);
                    if (!productResult.IsSuccess || productResult.Data == null)
                    {
                        return Result<decimal>.Fail("Failed to fetch product information.");
                    }
                    totalPrice += productResult.Data.Price;
                }
                return Result<decimal>.Success(totalPrice);
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Result<decimal>.Fail("Failed to calculate total amount: " + ex.Message);
            }
        }
        public async Task<Result<string>> ClearCartAsync(int cartId)
        {
            try
            {
                // Find cart items by cart ID
                var cartItems = _context.CartShoes.Where(item => item.CartId == cartId);

                // Remove cart items from database
                _context.CartShoes.RemoveRange(cartItems);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Result<string>.Success("Cart cleared successfully.");
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Result<string>.Fail("Failed to clear cart: " + ex.Message);
            }
        }
        public async Task<Result<List<int>>> GetProductIdsFromCartShoes(int cartId)
        {
            try
            {
                // Query cart shoes by cart ID
                var cartShoes = await _context.CartShoes
                    .Where(cs => cs.CartId == cartId)
                    .ToListAsync();

                // Extract product IDs from cart shoes
                var productIds = cartShoes.Select(cs => cs.ShoeId).ToList();

                return Result<List<int>>.Success(productIds);
            }
            catch (Exception ex)
            {
                // Log exception if needed
                // Return error result with message
                return Result<List<int>>.Fail("Failed to retrieve product IDs from cart shoes: " + ex.Message);
            }
        }


    }
}
