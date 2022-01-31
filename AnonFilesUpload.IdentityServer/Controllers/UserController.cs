using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.IdentityServer.Dtos;
using AnonFilesUpload.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace AnonFilesUpload.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    // Default gelen Policy'yi ekledik. Bu sayede token olmadan buraya istek yapılamayacaktır.
    // Token olmadan istek yapıldığında 401 hatası vereceğiz. 

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserController(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto dto)
        {

            var user = new ApplicationUser()
            {
                UserName = dto.UserName,
                City = dto.City,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                _logger.Write($"{dto.Email} oluşturulurken hata alındı");
                return BadRequest(result.Errors.ToList());
            }
            _logger.Write($"{dto.Email} başarıyla oluşturuldu");


            return NoContent();


        }
    }
}
