using AnonFilesUpload.Data.Models;
using AnonFilesUpload.MVC.Hubs;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services;
using AnonFilesUpload.MVC.Services.Interfaces;
using AnonFilesUpload.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace AnonFilesUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
      
        private readonly ISharedIdentityService _sharedIdentityService;

        private IHubContext<HubTest> _hubContext;

        public HomeController(IUserService userService, IHubContext<HubTest> hubContext, ISharedIdentityService sharedIdentityService)
        {
            _userService = userService;
            _hubContext = hubContext;
            _sharedIdentityService = sharedIdentityService;
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
                var data = await _userService.Upload(file, "data");
                model.Add(data.Data);
                await _hubContext.Clients.All.SendAsync("filesUploaded", data.Data);
            }

            return Json(new { finish = true });

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var deserializeData = await _userService.GetGenericAsync("data");
            var serializeData = JsonConvert.DeserializeObject<DataViewModel>(deserializeData);

            
            return await Task.FromResult(View(serializeData));
        }

        public async Task<IActionResult> Files()
        {
            var deserializeData = await _userService.GetGenericAsync("data/myfiles");
            var serializeData = JsonConvert.DeserializeObject<DataViewModel>(deserializeData);

            return await Task.FromResult(View("List", serializeData));
        }

        [HttpGet]
        public async Task<IActionResult> GetLink(string id)
        {
            var data = await _userService.GetGenericAsync($"data/getdirect/{id}");

            if (String.IsNullOrEmpty(data))
            {
                return RedirectToAction("Error");
            }

            return Redirect(data);
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
