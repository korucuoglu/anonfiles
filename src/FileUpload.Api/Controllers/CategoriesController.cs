using FileUpload.Api.Filters;
using FileUpload.Api.Services;
using FileUpload.Data.Entity;
using FileUpload.Shared.Dtos.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _categoriesService.GetAllAsync();

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _categoriesService.GetByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute<Category>))]
        public async Task<IActionResult> AddAsync(AddCategoryDto dto)
        {
            var data = await _categoriesService.AddAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        [ServiceFilter(typeof(ValidationFilterAttribute<Category>))]
        public async Task<IActionResult> UpdateAsync(string id, UpdateCategory dto)
        {
            var data = await _categoriesService.UpdateAsync(id, dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var data = await _categoriesService.DeleteByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }
    }
}
