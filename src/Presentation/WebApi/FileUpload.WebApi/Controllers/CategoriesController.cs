using FileUpload.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var query = new GetAllCategoriesByUserIdQueryRequest();

            return Ok(await _mediator.Send(query));

        }



        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    var data = await _categoriesService.GetByIdAsync(id);

        //    return new ObjectResult(data)
        //    {
        //        StatusCode = data.StatusCode
        //    };

        //}

        //[HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute<Category>))]
        //public async Task<IActionResult> AddAsync(AddCategoryDto dto)
        //{
        //    var data = await _categoriesService.AddAsync(dto);

        //    return new ObjectResult(data)
        //    {
        //        StatusCode = data.StatusCode
        //    };
        //}

        //[HttpPut("{id}")]
        //[ServiceFilter(typeof(NotFoundFilter<Category>))]
        //[ServiceFilter(typeof(ValidationFilterAttribute<Category>))]
        //public async Task<IActionResult> UpdateAsync(string id, UpdateCategory dto)
        //{
        //    var data = await _categoriesService.UpdateAsync(id, dto);

        //    return new ObjectResult(data)
        //    {
        //        StatusCode = data.StatusCode
        //    };

        //}

        //[HttpDelete("{id}")]
        //[ServiceFilter(typeof(NotFoundFilter<Category>))]
        //public async Task<IActionResult> DeleteByIdAsync(Guid id)
        //{
        //    var data = await _categoriesService.DeleteByIdAsync(id);

        //    return new ObjectResult(data)
        //    {
        //        StatusCode = data.StatusCode
        //    };

        //}
    }
}
