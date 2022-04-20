using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.Shared.Dtos.User;
using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService ıdentityService)
        {
            _identityService = ıdentityService;
        }

        [HttpGet("[controller]/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("[controller]/login")]
        public async Task<IActionResult> Login([FromBody] SigninInput model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _identityService.SignIn(model);

            return Ok(response);
        }

        [HttpGet("[controller]/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("[controller]/register")]
        public async Task<IActionResult> Register([FromBody] SignupInput model)
        {
            if (ModelState.IsValid is false)
            {
                return View();
            }

            var response = await _identityService.SignUp(model);

            return Ok(response);

        }


        [HttpGet("[controller]/info")]
        public IActionResult Info()
        {
            return View();
        }

        [HttpGet("[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet("[controller]/confirmEmail")]
        public async Task<IActionResult> ValidateUserEmail([FromQuery] string userId, string token)
        {
            var result = await _identityService.ValidateUserEmail(userId, token);

            return Ok(result);
        }

        [HttpGet("[controller]/reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return await Task.FromResult(View());

        }

        [HttpPost("[controller]/reset-password/{mail}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string mail)
        {
            if (string.IsNullOrEmpty(mail))
            {
                return Ok(Response<NoContent>.Fail("mail bilgisi boş olamaz", 404));
            }

            var data = await _identityService.ResetPassword(mail);

            return Ok(data);
        }

        [HttpGet("[controller]/reset-passwordConfirm")]
        public async Task<IActionResult> ResetPasswordConfirm([FromQuery][Bind("UserId, Token")] ResetPasswordConfirmModel model)
        {
            return await Task.FromResult(View(model));
        }

        [HttpPost("[controller]/reset-passwordConfirm")]
        public async Task<IActionResult> PostResetPasswordConfirm([FromBody] ResetPasswordConfirmModel model)
        {
            var data = await _identityService.ResetPasswordConfirm(model);

            return Ok(data);
        }

    }
}
