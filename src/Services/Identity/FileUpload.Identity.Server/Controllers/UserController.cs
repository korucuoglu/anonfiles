using FileUpload.Data.Entity;
using FileUpload.Shared.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FileUpload.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    // Default gelen Policy'yi ekledik. Bu sayede token olmadan buraya istek yapılamayacaktır.
    // Token olmadan istek yapıldığında 401 hatası vereceğiz. 

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupInput dto)
        {
            var user = new ApplicationUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                UserInfo = new()
                {
                    Id = Guid.NewGuid(),
                    UsedSpace = 0,
                }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }

            return NoContent();

        }


    }
}
