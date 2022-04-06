using FileUpload.Upload.Filters;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Base;
using FileUpload.Shared.Dtos.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Services;

namespace FileUpload.Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseApiController
    {
        private readonly IFileService _service;
        private readonly IHashService _hashService;

        public FilesController(IFileService service, IHashService hashService)
        {
            _service = service;
            _hashService = hashService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files, [FromForm] string categories)
        {
            List<int> categoryIds = string.IsNullOrEmpty(categories) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(categories);

            List<Response<AddFileDto>> result = new();

            foreach (var file in files)
            {
                var data = await _service.UploadAsync(file, categoryIds);

                result.Add(data);
            }
            return Ok(result);

        }

        [HttpPost("myfiles")]
        public async Task<IActionResult> GetAllAsync([FromBody] FileFilterModel model)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.GetAllFiles(filterModel);

            return Result(data);
        }

        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        [HttpGet("myfiles/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _service.GetFileById(hasId);

            return Result(data);

        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _service.Remove(hasId);

            return Result(data);

        }

        [HttpGet("download/{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Download(string id)
        {
            int hasId = _hashService.Decode(id);

            var data = await _service.Download(hasId);

            return Result(data);

        }
    }
}
