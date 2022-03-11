using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.MVC.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUserService _userService;

        public CategoriesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto dto)
        {
            await _userService.AddCategory(dto);

            return Json(new { finish = true });
        }
    }
}
