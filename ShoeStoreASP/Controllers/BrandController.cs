using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Service.Abstract;
using ShoeStoreASP.Service.Implementation;

namespace ShoeStoreASP.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public IActionResult GetAllBrands()
        {
            try
            {
                var result = _brandService.GetAllBrand();

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


        public IActionResult AddBrand()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            try
            {
                var result = _brandService.Add(brand);

                if (result.IsSuccess)
                {

                    TempData["msg"] = "Added Successully";
                    return RedirectToAction(nameof(AddBrand));
                    
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
        public IActionResult EditBrand(int id)
        {
            try
            {
                var brand = _brandService.GetBrandById(id);

                if (brand.IsSuccess)
                {
                    return View(brand.Data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult EditBrand(Brand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _brandService.UpdateBrand(model);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("GetAllBrands", "Brand"); // Chuyển hướng sau khi cập nhật thành công
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error");
            }
        }

      
      

        [HttpPost]
        public IActionResult ConfirmDeleteBrand(int id)
        {
            try
            {
                var result = _brandService.DeleteBrand(id);

                if (result.IsSuccess)
                {
                    return RedirectToAction("GetAllBrands", "Brand");  // Chuyển hướng sau khi xóa thành công
                }
                else
                {
                    return StatusCode(500, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
