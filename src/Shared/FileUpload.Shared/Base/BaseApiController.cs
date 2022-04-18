using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Shared.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        [NonAction]
        public IActionResult Result<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
