using AnonFilesUpload.Data.Models;
using AnonFilesUpload.MVC.Hubs;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services;
using AnonFilesUpload.MVC.Services.Interfaces;
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
        private readonly ILogger<HomeController> _logger;

        private readonly IApiService _apiService;
        private readonly IIdentityService _identityService;

        private IHubContext<HubTest> _hubContext;

        public HomeController(ILogger<HomeController> logger, IApiService apiService, IHubContext<HubTest> hubContext, IIdentityService identityService)
        {
            _logger = logger;
            _apiService = apiService;
            _hubContext = hubContext;
            _identityService = identityService;
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
                var data = await _apiService.Upload(file, $"data?UserId={_identityService.GetUserId}");
                model.Add(data.Data);
                await _hubContext.Clients.All.SendAsync("filesUploaded", data.Data);
            }

            return Json(new { finish = true });

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var deserializeData = await _apiService.GetGenericAsync("data");
            var serializeData = JsonConvert.DeserializeObject<DataViewModel>(deserializeData);

            
            return await Task.FromResult(View(serializeData));
        }

        public async Task<IActionResult> Files()
        {
            var deserializeData = await _apiService.GetGenericAsync($"data/?UserId={_identityService.GetUserId}");
            var serializeData = JsonConvert.DeserializeObject<DataViewModel>(deserializeData);

            return await Task.FromResult(View("List", serializeData));
        }

        [HttpGet]
        public async Task<IActionResult> GetLink(string id)
        {
            var data = await _apiService.GetGenericAsync($"data/getdirect/{id}");

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
