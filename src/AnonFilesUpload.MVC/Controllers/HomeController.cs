﻿using AnonFilesUpload.Shared.Services;
using AnonFilesUpload.MVC.Hubs;
using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using AnonFilesUpload.MVC.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace AnonFilesUpload.MVC.Controllers
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
        public async Task<IActionResult> Upload(IFormFile[] files)
        {
            var ConnectionId = HubData.ClientsData.Where(x => x.UserId == _sharedIdentityService.GetUserId).Select(x => x.ConnectionId).FirstOrDefault();

            foreach (var file in files)
            {
                await _fileHub.Clients.Client(ConnectionId).FilesUploadStarting(file.FileName);
                var data = await _userService.Upload(file);
                await _fileHub.Clients.Client(ConnectionId).FilesUploaded(data);
            }

            return Json(new { finish = true });

        }


        [HttpGet]
        [Route("myfiles")]

        public async Task<IActionResult> Files()
        {
            var data = await _userService.GetMyFiles();
            return await Task.FromResult(View(data.Data));
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _userService.DeleteFile(id);

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