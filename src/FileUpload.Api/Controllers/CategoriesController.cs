using AutoMapper;
using FileUpload.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _categoriesService;

        public CategoriesController(CategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var data = await _categoriesService.GetAllCategories();
            return Ok(data);

        }
    }
}
