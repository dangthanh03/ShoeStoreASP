using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Service.Abstract;
using System.Diagnostics;

namespace ShoeStoreASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoeService _shoeService;
        private readonly DatabaseContext _context;
        public HomeController(ILogger<HomeController> logger, IShoeService shoeService,DatabaseContext databaseContext)
        {
            _context = databaseContext;
            _logger = logger;
            _shoeService = shoeService;
        }

        public IActionResult Index(string term = "", int currentPage = 1, string selectedTypes = null, int BrandId=-1)
        {
            try
            {
                // Chuyển selectedTypes từ chuỗi thành một danh sách các số nguyên
                List<int> selectedTypeIds = new List<int>();
                if (!string.IsNullOrEmpty(selectedTypes))
                {
                    selectedTypeIds = selectedTypes.Split(',').Select(int.Parse).ToList();
                }

                var result = _shoeService.GetAllShoes(term, true, currentPage, selectedTypeIds,BrandId);
                result.Data.AllTypes = _context.Types.ToList();
                result.Data.AllBrands = _context.Brands.ToList();

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
                // Log lỗi nếu cần
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        public IActionResult Search(string term = "", int currentPage = 1, List<int> selectedTypes = null, int BrandId = -1)
        {
            try
            {
                
                
                var result = _shoeService.GetAllShoes(term, true, currentPage, selectedTypes,BrandId);
                result.Data.AllTypes = _context.Types.ToList();
                result.Data.AllBrands = _context.Brands.ToList();

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
                // Log lỗi nếu cần
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
