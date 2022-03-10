using FileUpload.Data.Entity;
using FileUpload.Shared.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
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

            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            UserCreatedEvent userCreatedEvent = new()
            {
                MailAdress = user.Email,
                Message = $"{configuration.GetSection("MVCClient").Value}?userId={user.Id}&token={emailConfirmationToken}"
            };

            return NoContent();

        }

        [HttpGet]
        public async Task<bool> ValidateUserEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;

        }
    }
}
