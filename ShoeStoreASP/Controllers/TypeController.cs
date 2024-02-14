using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Controllers
{
    public class TypeController : Controller
    {
        private readonly ITypeService _typeService;
        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }
       
        public IActionResult AddType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddType(ShoeStoreASP.Models.Domain.Type type)
        {
            if (ModelState.IsValid)
            {
                // Gọi service hoặc repository để thêm mới Type vào database
                var result = _typeService.AddType(type);

                if (result.IsSuccess)
                {
                    return RedirectToAction("GetAllType", "Type");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            return View(type);
        }

        public IActionResult GetAllType()
        {
            try
            {
                var result = _typeService.GetAllTypes();

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
        
        public IActionResult EditType(int id)
        {
            var result = _typeService.GetTypeById(id);

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                return NotFound(new { message = result.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateType(ShoeStoreASP.Models.Domain.Type type)
        {
            var result = _typeService.UpdateType(type);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetAllType","Type"); // Chuyển hướng đến danh sách Type sau khi cập nhật thành công
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteTypeConfirmed(int id)
        {
            var result = _typeService.DeleteType(id);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetAllType", "Type"); // Chuyển hướng đến danh sách Type sau khi xóa thành công
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }



    }
}
