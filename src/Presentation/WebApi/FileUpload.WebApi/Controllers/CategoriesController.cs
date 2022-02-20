using FileUpload.Api.Filters;
using FileUpload.Application.Features.Commands.Categories.Add;
using FileUpload.Application.Features.Commands.Categories.Update;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Domain.Entities; // yanlış
using FileUpload.Infrastructure.Attribute;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileUpload.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            return Ok(await _categoryService.GetAllAsync());

        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _categoryService.GetByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddAsync(AddCategoryCommand dto)
        {
            var data = await _categoryService.AddAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryCommand dto)
        {
            var data = await _categoryService.UpdateAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var data = await _categoryService.DeleteByIdAsync(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }
    }
}
