
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IFileService _fileService;

        private readonly IConfiguration _configuration;
        private readonly ISharedIdentityService _sharedIdentityService;



        public DataController(IFileService fileService, IConfiguration configuration, ISharedIdentityService sharedIdentityService)
        {
            _fileService = fileService;
            _configuration = configuration;
            _sharedIdentityService = sharedIdentityService;
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

        [HttpGet("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {

            if (User.Identity.IsAuthenticated)
            {
                return Ok(_sharedIdentityService.GetUserId);
            }

            return Ok(_configuration.GetSection("token").Value);
        }

      
        [HttpGet("myfiles")]
        public async Task<IActionResult> GetMyFiles()
        {

            var data = await _fileService.GetMyFiles();

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
