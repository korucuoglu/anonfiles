using FileUpload.Upload.Filters;
using FileUpload.Upload.Application.Features.Commands.Categories.Add;
using FileUpload.Upload.Application.Features.Commands.Categories.Update;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Infrastructure.Attribute;
using FileUpload.Shared.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileUpload.Shared.Services;

namespace FileUpload.Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IHashService _hashService;

        public CategoriesController(ICategoryService categoryService, IHashService hashService)
        {
            _categoryService = categoryService;
            _hashService = hashService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _categoryService.GetAllAsync();

            return Result(data);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _categoryService.GetByIdAsync(hasId);

            return Result(data);

        }

        [HttpPost]
        [ValidationFilter]
        public async Task<IActionResult> AddAsync(AddCategoryCommand dto)
        {
            var data = await _categoryService.AddAsync(dto);

            return Result(data);
        }

        [ValidationFilter]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryCommand dto)
        {
            var data = await _categoryService.UpdateAsync(dto);

            return Result(data);

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> DeleteByIdAsync(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _categoryService.DeleteByIdAsync(hasId);

            return Result(data);

        }
    }
}
