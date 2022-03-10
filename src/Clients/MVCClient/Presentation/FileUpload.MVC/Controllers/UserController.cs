using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.Shared.Dtos.User;
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

        [Route("[controller]/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SigninInput model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _identityService.SignIn(model);

            if (!response)
            {

                ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı");

                return View();
            }

            return RedirectToAction("Upload", "Home");
        }

        [Route("[controller]/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignupInput model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var data = await _identityService.SignUp(model);

            if (data is false)
            {
                ModelState.AddModelError(string.Empty, "Hata meydana geldi.");

                return View();
            }

            return RedirectToAction("Index", "Home");


        }


        [Route("[controller]/info")]
        [HttpGet]
        public IActionResult Info()
        {
            return View();
        }



        [Route("[controller]/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


    }
}
