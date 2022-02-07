using FileUpload.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var data = await _service.UploadAsync(file);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }
    }
}
