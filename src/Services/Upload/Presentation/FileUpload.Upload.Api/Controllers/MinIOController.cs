using FileUpload.Upload.Filters;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Base;
using FileUpload.Shared.Dtos.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileUpload.Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinIOController : BaseApiController
    {
        private readonly IFileService _service;

        public MinIOController(IFileService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files, [FromForm] string categories)
        {
            var categoryIds = JsonSerializer.Deserialize<List<Guid>>(categories);

            var data = await _service.UploadAsync(files, categoryIds);

            return Response(data);

        }

        [HttpPost("myfiles")]
        public async Task<IActionResult> GetAllAsync([FromBody] FileFilterModel model)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.GetAllFiles(filterModel);

            return Response(data);
        }

        [HttpGet("myfiles/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _service.GetFileById(id);

            return Response(data);

        }

        [HttpPost("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove([FromBody] FileFilterModel model, Guid id)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.Remove(filterModel, id);

            return Response(data);

        }

        [HttpGet("download/{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Download(string id)
        {
            var data = await _service.Download(id);

            return Response(data);

        }
    }
}
