using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly IRepository<File> _fileService;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly ISharedIdentityService _sharedIdentityService;


        public TestController(IRepository<File> fileService, UserManager<ApplicationUser> userService, ISharedIdentityService sharedIdentityService)
        {
            _fileService = fileService;
            _userService = userService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetMyFiles()
        {

            var data = await _userService.FindByIdAsync(_sharedIdentityService.GetUserId);

            return Ok(data);

        }


    }
}
