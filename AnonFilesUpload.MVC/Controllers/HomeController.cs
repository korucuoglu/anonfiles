using AnonFilesUpload.Shared.Models;
using AnonFilesUpload.MVC.Hubs;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace AnonFilesUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private IHubContext<HubTest> _hubContext;

        public HomeController(IUserService userService, IHubContext<HubTest> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile[] files)
        {

            var model = new List<AjaxReturningModel>();

            foreach (var file in files)
            {
                await _hubContext.Clients.All.SendAsync("filesUploadedStarting", file.FileName);
                var data = await _userService.Upload(file);
                model.Add(data.Data);
                await _hubContext.Clients.All.SendAsync("filesUploaded", data.Data);
            }

            return Json(new { finish = true });

        }

        
        public async Task<IActionResult> Files()
        {
            var data = await _userService.GetMyFiles();
            return await Task.FromResult(View("List", data.Data));
        }

        [HttpGet]
        public async Task<IActionResult> GetLink(string id)
        {
            var data = await _userService.GetDirectLink(id);

            if (!data.IsSuccessful)
            {
                return RedirectToAction("Error");
            }

            return Redirect(data.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
