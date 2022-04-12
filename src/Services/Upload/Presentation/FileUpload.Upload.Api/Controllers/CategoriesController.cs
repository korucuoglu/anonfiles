using FileUpload.Upload.Filters;
using FileUpload.Upload.Application.Features.Commands.Categories;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Infrastructure.Attribute;
using FileUpload.Shared.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileUpload.Shared.Services;
using MediatR;
using FileUpload.Upload.Application.Features.Queries.Categories;

namespace FileUpload.Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHashService _hashService;

        public CategoriesController(IMediator mediator, IHashService hashService)
        {
            _mediator = mediator;
            _hashService = hashService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _mediator.Send(new GetAllCategoriesQueryRequest());

            return Result(data);
        }

        [HttpGet("{id}")]
        // [ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _mediator.Send(new GetCategoryByIdQueryRequest() { Id = hasId });

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
        //[ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryCommand model)
        {
            var data = await _mediator.Send(model);

            return Result(data);

        }

        [HttpDelete("{id}")]
        //[ServiceFilter(typeof(NotFoundFilterAttribute<Category>))]
        public async Task<IActionResult> DeleteByIdAsync(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _mediator.Send(new DeleteCategoryCommand() { Id = hasId});

            return Result(data);

        }
    }
}
