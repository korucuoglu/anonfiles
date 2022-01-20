
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Shared.Models;
using AnonFilesUpload.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly DataContext _context;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DataController(FileService fileService, DataContext context, ISharedIdentityService sharedIdentityService)
        {
            _fileService = fileService;
            _context = context;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var data = await _fileService.UploadAsync(file, _sharedIdentityService.GetUserId);

            return Ok(data);
        }


        [HttpGet("myfiles")]
        public async Task<IActionResult> GetFilesByUserId()
        {

            var data = await _context.Data.Where(x => x.UserId == _sharedIdentityService.GetUserId).Select(x => new DataModel()
            {
                FileId = x.MetaDataId,
                FileName = x.Name,
                Size = x.Size

            }).ToListAsync();

            DataViewModel model = new()
            {
                DataModel = data,
                TotalSize = data.Sum(x => x.Size),
                UsedSpace = Helper.GetUsedSpace(data.Sum(x => x.Size)),
                RemainingSpace = Helper.GetRemainingSpace(data.Sum(x => x.Size)),

            };

            return Ok(model);

        }

        [HttpGet("getdirect/{id}")]
        public async Task<IActionResult> GetDirectLinkByMetaDataIdAsync(string id)
        {
            // MetaData Id üzerinden ShortUri elde edilir. 
            // Short Üri üzerinden DirectLink elde edilierek geriye dönülür. 


            string shortUri = "https://anonfiles.com/" + id;

            var data = await _fileService.GetDirectLinkAsync(shortUri);

            return Ok(data);
        }

       
    }
}
