﻿
using AnonFilesUpload.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IFileService _fileService;

        public DataController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var data = await _fileService.UploadAsync(file);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetFilesByUserId()
        {

            var data = await _fileService.GetFilesByUserId();

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectLinkByMetaDataIdAsync(string id)
        {
            var data = await _fileService.GetDirectLinkByMetaId(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _fileService.DeleteAsyncByMetaId(id);

            return Ok(data);
           
        }
       
    }
}
