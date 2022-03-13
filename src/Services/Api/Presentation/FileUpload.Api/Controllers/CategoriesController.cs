using FileUpload.Api.Filters;
using FileUpload.Application.Features.Commands.Categories.Add;
using FileUpload.Application.Features.Commands.Categories.Update;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Domain.Entities;
using FileUpload.Infrastructure.Attribute;
using FileUpload.Shared.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _categoryService.GetAllAsync();

            return Response(data);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _categoryService.GetByIdAsync(id);

            return Response(data);

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddAsync(AddCategoryCommand dto)
        {
            var data = await _categoryService.AddAsync(dto);

            return Response(data);
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryCommand dto)
        {
            var data = await _categoryService.UpdateAsync(dto);

            return Response(data);

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var data = await _categoryService.DeleteByIdAsync(id);

            return Response(data);

        }
    }
}
