using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Shared.Base
{
    public class BaseApiController: ControllerBase
    {
        [NonAction]
        public IActionResult Response<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
