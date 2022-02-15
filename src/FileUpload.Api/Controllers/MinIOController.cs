using FileUpload.Api.Filters;
using FileUpload.Api.Services;
using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinIOController : ControllerBase
    {
        private readonly MinIOService _service;

        public MinIOController(MinIOService service)
        {
            _service = service;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute<UploadModel>))]
        public async Task<IActionResult> Upload([FromForm] UploadFileDto dto)
        {
            var data = await _service.UploadAsync(dto);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetMyFiles([FromQuery] FileFilterModel model)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.GetMyFiles(filterModel);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("download/{id}")] 
        [ServiceFilter(typeof(NotFoundFilter<Data.Entity.File>))]
        public async Task<IActionResult> Download(string id)
        {
            var data = await _service.Download(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Data.Entity.File>))]
        public async Task<IActionResult> Remove([FromQuery] FileFilterModel model, string id)
        {
            var data = await _service.Remove(model, id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }


    }
}
