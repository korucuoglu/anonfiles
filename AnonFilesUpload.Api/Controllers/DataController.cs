
using AnonFilesUpload.Api.Services;
using AnonFilesUpload.Data.Entity;
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
            if (file.Length < 0)
                return BadRequest("Boş veri girilemez");

            var data = await _fileService.UploadAsync(file);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Data.ToListAsync();

            return Ok(data);
        }

        [HttpGet("control/{id}")]
        public async Task<IActionResult> Control(string id)
        {
            var DataEntity = await _context.Data.Where(x => x.MetaDataId == id).FirstOrDefaultAsync();
            var data = await _fileService.DirectLinkControl(DataEntity);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _context.Data.FirstOrDefaultAsync(x => x.MetaDataId == id);

            return Ok(data);
        }

        [HttpGet("direct/{id}")]
        public async Task<IActionResult> GetDirectLink(string id)
        {
            string shortUri = "https://anonfiles.com/" + id;

            var data = await _fileService.GetDirectLinkAsync(shortUri);

            return Ok(data);
        }

        

        
    }
}
