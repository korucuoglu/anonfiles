using FileUpload.Api.Filters;
using FileUpload.Api.Services;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Upload([FromForm] UploadFileDto2 dto)
        {
            var category = JsonConvert.DeserializeObject<List<GetCategoryDto>>(dto.Categories);

            UploadFileDto fileDto = new()
            {
                Categories = category,
                Files = dto.Files
            };

            // var data = Response<UploadModel>.Fail($"kategori sayısı: {category.Count}", 500);
            
            // var data = Response<UploadModel>.Fail($"kategori sayısı: {dto.Categories.Count}", 500);

            var data = await _service.UploadAsync(fileDto);

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
        public async Task<IActionResult> Remove([FromQuery] FileFilterModel model, Guid id)
        {
            var data = await _service.Remove(model, id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }


    }
}
