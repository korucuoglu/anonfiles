using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.MVC.Application.Exceptions;
using FileUpload.MVC.Application.Dtos.Error;
using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Wrappers;

namespace FileUpload.MVC.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return await Task.FromResult(View());
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
        
    }
}
