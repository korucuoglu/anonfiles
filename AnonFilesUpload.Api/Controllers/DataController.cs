
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Shared.Models;
using AnonFilesUpload.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
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

            return Ok(data);

        }

        [HttpGet("getdirect/{id}")]
        public async Task<IActionResult> GetDirectLinkByMetaDataIdAsync(string id)
        {
            var data = await _fileService.GetDirectLinkByMetaId(id);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };

        }

        [HttpDelete("{MetaId}")]
        public async Task<IActionResult> Delete(string MetaId)
        {
            var data = await _fileService.DeleteAsyncByMetaId(MetaId);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
           
        }
       
    }
}
