using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Identity.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepository<UserInfo> _repository;

        public UserController(UserManager<ApplicationUser> userManager, IRepository<UserInfo> repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
       
        [HttpPost]
        public async Task<IActionResult> Signup(SignupInput dto)
        {

            var user = new ApplicationUser()
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }

            var UserInfo = new UserInfo()
            {
                ApplicationUserId = user.Id,
            };

            await _repository.AddAsync(UserInfo);

            return NoContent();

        }

        
    }
}
