
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public DataController(FileService fileService, DataContext context)
        {
            _fileService = fileService;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var data = await _fileService.UploadAsync(file);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _context.Data.Select(x => new DataModel()
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            // MetaDataId üzerinden filtreleme yapılır. 

            var data = await _fileService.GetById<Data.Entity.Data>(id);

            return Ok(data);
        }

        [HttpGet("direct/{id}")]
        public async Task<IActionResult> ReturnDirectLinkByMetaDataIdAsync(string id)
        {
            // MetaData Id üzerinden ShortUri elde edilir. 
            // Short Üri üzerinden DirectLink elde edilierek geriye dönülür. 


            string shortUri = "https://anonfiles.com/" + id;

            var data = await _fileService.GetDirectLinkAsync(shortUri);

            return Redirect(data);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _fileService.DeleteByIdAsync<Data.Entity.Data>(id);
            return NotFound(data);

        }


        //[HttpGet("control/{id}")]
        //public async Task<IActionResult> ControlAsync(string id)
        //{
        //    // MetaDataId üzerinden DirectLinkControl edilir. 
        //    // Kontrol sonucunda link güncel değilse güncellenir. 

        //    var DataEntity = await _context.Data.Where(x => x.MetaDataId == id).FirstOrDefaultAsync();
        //    var data = await _fileService.DirectLinkControl(DataEntity);
        //    return Ok(data);
        //}
    }
}
