using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Controllers
{
    public class ShoeController : Controller
    {
        private readonly ICartService _cartService;

        private readonly IShoeService _shoeService;
        private readonly IFileService _fileService;
        private readonly DatabaseContext _context;
         private readonly UserManager<ApplicationUser> _userManager;

        public ShoeController(IShoeService shoeService, IFileService fileService, DatabaseContext databaseContext, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _shoeService = shoeService;
            _fileService = fileService;
            _context = databaseContext;
            _userManager = userManager;
        }
        // Trong ShoeController.cs hoặc controller tương ứng
        public IActionResult ProductDetails(int id)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu dựa trên id
            var shoe =_shoeService.GetShoeById(id);

            if (!shoe.IsSuccess)
            {
                // Xử lý trường hợp sản phẩm không tồn tại
                return RedirectToAction("Index"); // Hoặc thực hiện hành động khác tùy thuộc vào yêu cầu của bạn
            }

            return View(shoe.Data);
        }


        public IActionResult AddShoe()
        {
            var brands = _context.Brands.ToList();
            var allTypes = _context.Types.ToList();

            var viewModel = new AddShoeVM

            {
                Brands = brands,
                AllTypes = allTypes
            };

            return View(viewModel);

        }
        [HttpPost]
        public IActionResult AddShoe(AddShoeVM model)
        {
            


            var result = _shoeService.AddShoe(model);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetAllShoes","Shoe"); 
            }
            else
            {
             
                ModelState.AddModelError("Add Shoe:", result.Message);
            }

   
            model.Brands = _context.Brands.ToList();
            model.AllTypes = _context.Types.ToList();
            return View(model);
        }


        [HttpGet]
        public IActionResult GetAllShoes()
        {
            try
            {
                var result = _shoeService.GetAllShoes();

                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult DeleteShoe(int id)
        {
            var result = _shoeService.DeleteShoe(id);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetAllShoes", "Shoe");
            }
            else
            {
                // Xử lý lỗi, có thể redirect hoặc hiển thị thông báo lỗi
                return RedirectToAction("Error", "Home");
            }
        }

       
        public IActionResult EditShoe(int id)
        {
            var result = _shoeService.GetShoeById(id);
            var brands = _context.Brands.ToList();
            var allTypes = _context.Types.ToList();
            var shoe = result.Data;
            if (shoe == null)
            {
                return NotFound();
            }

            var viewModel = new EditShoeVm
            {
                ShoeId = shoe.ShoeId,
                Name = shoe.Name,
                BrandId = shoe.BrandId,
              Brands = brands,
              AllTypes = allTypes,
                Price = shoe.Price,
                StockQuantity = shoe.StockQuantity,
                Description = shoe.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditShoe(EditShoeVm model)
        {
            

            var result = _shoeService.EditShoe(model);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetAllShoes", "Shoe");
            }
            else
            {
                ModelState.AddModelError("Edit Shoe:", result.Message);
            }

           
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangeImg(int id)
        {
            // Hiển thị trang để chọn file
            var shoe = _shoeService.GetShoeById(id);
            var changeImg = new ChangeImgVm
            {
                ShoeId = shoe.Data.ShoeId
            };
            return View(changeImg);
        }

        [HttpPost]
        public IActionResult ChangeImg(ChangeImgVm shoe)
        {
            // Xử lý tệp đã được gửi lên
            if (shoe.formFile != null && shoe.formFile.Length > 0)
            {
                _shoeService.UpdateImg(shoe.formFile, shoe.ShoeId);
                return RedirectToAction("GetAllShoes","Shoe");
            }

            // Nếu không có tệp được gửi lên, có thể hiển thị thông báo lỗi hoặc quay lại trang với thông báo.
            ModelState.AddModelError("ImageFile", "Please select a file.");

            return View();
        }

    }
}

