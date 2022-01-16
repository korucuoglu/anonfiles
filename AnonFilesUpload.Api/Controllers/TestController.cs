using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnonFilesUpload.Api.Hubs;
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AnonFilesUpload.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IHubContext<HubTestApi> _hubContext;
        private readonly FileService _fileService;

        public TestController(IHubContext<HubTestApi> hubContext, FileService fileService)
        {
            _hubContext = hubContext;
            _fileService = fileService;
        }

        // [HttpPost]
        // public async Task<IActionResult> Test(IFormFile[] files)
        // {
        //     var ListModel = new List<AjaxReturningModel>();

        //     Random _random = new Random();

        //     foreach (var file in files)
        //     {
        //         var model = new AjaxReturningModel()
        //         {
        //             fileId = _random.Next(111111, 999999).ToString(),
        //             fileName = file.FileName,
        //             success = true,
        //             message = $"{file.FileName} başarıyla yüklendi",
        //         };

        //         ListModel.Add(model);
        //     }

        //     return await Task.FromResult(Ok(ListModel));
        // }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile[] files)
        {
            var model = new List<AjaxReturningModel>();

            foreach (var file in files)
            {
                await _hubContext.Clients.All.SendAsync("filesUploadedStarting", file.FileName);
                var data = await _fileService.UploadAsync(file);
                model.Add(data);
                await _hubContext.Clients.All.SendAsync("filesUploaded", data);
            }

            return await Task.FromResult(Ok(model));
        }

    }
}