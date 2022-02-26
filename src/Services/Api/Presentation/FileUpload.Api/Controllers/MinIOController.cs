using FileUpload.Api.Filters;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Domain.Entities;
using FileUpload.Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinIOController : ControllerBase
    {

        private readonly IFileService _service;

        public MinIOController(IFileService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files, [FromForm] string categories)
        {
            var categoryIds = new List<Guid>();

            if (String.IsNullOrEmpty(categories) is false)
            {
                categoryIds = JsonSerializer.Deserialize<List<Guid>>(categories);
            }

            var data = await _service.UploadAsync(files, categoryIds);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpPost("myfiles")]
        public async Task<IActionResult> GetAllAsync([FromBody] FileFilterModel model)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.GetAllFiles(filterModel);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpGet("myfiles/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _service.GetFileById(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpPost("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove([FromBody] FileFilterModel model, Guid id)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.Remove(filterModel, id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("download/{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Download(string id)
        {
            var data = await _service.Download(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

    }
}
