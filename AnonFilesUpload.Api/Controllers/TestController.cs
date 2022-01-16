using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonFilesUpload.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

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

            return await Task.FromResult(Ok(ListModel));
        }

    }
}