using FileUpload.Api.Dtos.File;
using FileUpload.Api.Filters;
using FileUpload.Api.Services;
using FileUpload.Data.Entity;
using FileUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
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
            var data = await _service.UploadAsync(dto.File);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetMyFiles(int page, int number, int orderBy, string extension)
        {
            var data = await _service.GetMyFiles(page, number, orderBy, extension);

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
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _service.Remove(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }


    }
}
