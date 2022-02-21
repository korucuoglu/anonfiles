using FileUpload.Api.Filters;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Features.Commands.Files.Add;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Domain.Entities;
using FileUpload.Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.WebApi.Controllers
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files)
        {
            var data = await _service.UploadAsync(files);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetAllAsync([FromQuery] FileFilterModel model)
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

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove([FromQuery] FileFilterModel model, Guid id)
        {
            FileFilterModel filterModel = new(model);

            var data = await _service.Remove(filterModel, id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

    }
}
