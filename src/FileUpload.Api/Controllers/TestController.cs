using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly IRepository<File> _fileService;
        private readonly IRepository<Category> _categoryService;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly ISharedIdentityService _sharedIdentityService;


        public TestController(IRepository<File> fileService, UserManager<ApplicationUser> userService, ISharedIdentityService sharedIdentityService, IRepository<Category> categoryService)
        {
            _fileService = fileService;
            _userService = userService;
            _sharedIdentityService = sharedIdentityService;
            _categoryService = categoryService;
        }

        [HttpGet("myfiles")]
        public async Task<IActionResult> GetMyFiles()
        {
            var user = await _userService.FindByIdAsync(_sharedIdentityService.GetUserId);
          
            return Ok(user);

        }


    }
}
