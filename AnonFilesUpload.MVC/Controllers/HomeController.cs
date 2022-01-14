using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory iHttpClientFactory, IApiService apiService)
        {
            _logger = logger;

            apiService.HttpClient = iHttpClientFactory.CreateClient("ApiServiceClient");

            _apiService = apiService;

        }

        public async Task<IActionResult> Index()
        {
            // return View("Privacy");

            var data = await _apiService.GetAllAsync<IEnumerable<Data.Entity.Data>>("data");

            return View(data);
        }

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
