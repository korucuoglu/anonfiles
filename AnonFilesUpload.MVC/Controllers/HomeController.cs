using AnonFilesUpload.Data.Models;
using AnonFilesUpload.MVC.Hubs;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace AnonFilesUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IApiService _apiService;

        private IHubContext<HubTest> _hubContext;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory iHttpClientFactory, IApiService apiService, IHubContext<HubTest> hubContext)
        {
            _logger = logger;
            apiService.HttpClient = iHttpClientFactory.CreateClient("ApiServiceClient");
            _apiService = apiService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public async Task<IActionResult> Test(IFormFile[] files)
        {
            var ListModel = new List<AjaxReturningModel>();

            Random _random = new Random();

            foreach (var file in files)
            {
                var model = new AjaxReturningModel()
                {
                    fileId = _random.Next(111111, 999999).ToString(),
                    fileName = file.FileName,
                    success = true,
                    message = $"{file.FileName} başarıyla yüklendi",
                };

                ListModel.Add(model);
            }

            return await Task.FromResult(Json(ListModel));
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile[] files)
        {

            var model = new List<AjaxReturningModel>();

            foreach (var file in files)
            {
                await _hubContext.Clients.All.SendAsync("filesUploadedStarting", file.FileName);
                var data = await _apiService.Upload(file, "data");
                model.Add(data.Data);
                await _hubContext.Clients.All.SendAsync("filesUploaded", data.Data);
            }

            return Json(new { finish = true });

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var data = await _apiService.GetAllAsync<DataViewModel>("data");
            return await Task.FromResult(View(data));
        }

        [HttpGet]
        public async Task<IActionResult> GetLink(string id)
        {

            var data = await _apiService.GetAsync($"data/direct/{id}");

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
