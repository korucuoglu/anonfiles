using AutoMapper;
using FileUpload.Api.Dtos.Categories;
using FileUpload.Api.Filters;
using FileUpload.Api.Services;
using FileUpload.Data.Entity;
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
            var data = await _categoriesService.GetAllAsync();

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var data = await _categoriesService.GetByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCategory(AddCategoryDto dto)
        {
            var data = await _categoriesService.AddAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCategory(UpdateCategory dto)
        {
            var data = await _categoriesService.UpdateAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> DeleteCategoryById(string id)
        {
            var data = await _categoriesService.DeleteByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }
    }
}
