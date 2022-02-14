using FileUpload.Shared.Services;
using FileUpload.MVC.Hubs;
using FileUpload.MVC.Models;
using FileUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using FileUpload.MVC.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using FileUpload.MVC.Services;
using FileUpload.Shared.Models.Files;

namespace FileUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHubContext<FileHub, IFileHub> _fileHub;
        private readonly ISharedIdentityService _sharedIdentityService;


        public HomeController(IUserService userService, IHubContext<FileHub, IFileHub> fileHub, ISharedIdentityService sharedIdentityService)
        {
            _userService = userService;
            _fileHub = fileHub;
            _sharedIdentityService = sharedIdentityService;

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
           
            return await Task.FromResult(View());
        }


        [HttpPost]
        public async Task<IActionResult> Upload(UploadFileDto dto)
        {
            var ConnectionId = HubData.ClientsData.Where(x => x.UserId == _sharedIdentityService.GetUserId).Select(x => x.ConnectionId).FirstOrDefault();

            foreach (var file in dto.Files)
            {
                await _fileHub.Clients.Client(ConnectionId).FilesUploadStarting(file.FileName);
                var data = await _userService.Upload(file);
                await _fileHub.Clients.Client(ConnectionId).FilesUploaded(data);
            }

            return Json(new { finish = true });

        }


        [HttpGet]
        [Route("myfiles")]
        public async Task<IActionResult> Files([FromQuery] FileFilterModel model, [FromQuery] bool? json)
        {
            FileFilterModel filterModel = new(model);

            var response = await _userService.GetMyFiles(filterModel);

            if (json==true)
            {
                return await Task.FromResult(Ok(response));
            }


            return await Task.FromResult(View(response.Data));
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
