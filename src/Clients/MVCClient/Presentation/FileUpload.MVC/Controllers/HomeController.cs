using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.MVC.Application.Dtos.Files;
using FileUpload.MVC.Application.Exceptions;
using FileUpload.MVC.Application.Dtos.Error;

namespace FileUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;


        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {

            return await Task.FromResult(View());
        }


        [HttpGet]
        [Route("upload")]
        [Authorize]
        public async Task<IActionResult> Upload()
        {
            var data = await _userService.GetCategories();

            return await Task.FromResult(View(data));
        }


        [HttpPost]
        public async Task<IActionResult> Upload(FilesCategoriesDto dto)
        {
            await _userService.Upload(dto);

            return Json(new { finish = true });
        }

        [HttpGet]
        [Route("myfiles")]
        public async Task<IActionResult> Files([FromQuery] FileFilterModel model, [FromQuery] bool? json)
        {
            var response = await _userService.GetMyFiles(model);

            if (json == true)
            {
                return await Task.FromResult(Ok(response));
            }

            return await Task.FromResult(View(response));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] FileFilterModel model, string id)
        {
            var data = await _userService.DeleteFile(model, id);

            return Ok(data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
            {
                return RedirectToAction(nameof(UserController.Login), "User");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetLink(string id)
        {
            var data = await _userService.GetDirectLink(id);


            return Ok(data);
        }
    }
}
