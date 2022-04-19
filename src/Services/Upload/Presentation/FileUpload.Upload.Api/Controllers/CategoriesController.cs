using FileUpload.Shared.Base;
using FileUpload.Upload.Application.Features.Commands.Categories;
using FileUpload.Upload.Application.Features.Queries.Categories;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Filters;
using FileUpload.Upload.Infrastructure.Attribute;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _mediator.Send(new GetAllCategoriesQueryRequest());

            return Result(data);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var data = await _mediator.Send(new GetCategoryByIdQueryRequest() { Id = id });

            return Result(data);
        }

        [HttpPost]
        [ValidationFilter]
        public async Task<IActionResult> AddAsync(AddCategoryCommand model)
        {
            var data = await _mediator.Send(model);

            return Result(data);
        }

        [ValidationFilter]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryCommand model)
        {
            var data = await _mediator.Send(model);

            return Result(data);

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> DeleteByIdAsync(string id)
        {
            var data = await _mediator.Send(new DeleteCategoryCommand() { Id = id });

            return Result(data);

        }
    }
}
