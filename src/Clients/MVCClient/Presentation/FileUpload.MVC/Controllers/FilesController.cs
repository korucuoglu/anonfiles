using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.Shared.Dtos.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUpload.MVC.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IUserService _userService;

        public FilesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[controller]/myfiles")]
        public async Task<IActionResult> Files()
        {
            var response = await _userService.GetMyFiles(new FileFilterModel());

            return await Task.FromResult(View(response));
        }


        [HttpPost("[controller]/myfiles")]
        public async Task<IActionResult> Files([FromBody] FileFilterModel model)
        {
            var response = await _userService.GetMyFiles(model);

            return await Task.FromResult(Ok(response));
        }


        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var data = await _userService.GetCategories();

            return await Task.FromResult(View(data));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FilesCategoriesDto dto)
        {
            await _userService.Upload(dto);

            return Json(new { finish = true });
        }


        [HttpGet("[controller]/download/{id}")]
        public async Task<IActionResult> DownloandFile(string id)
        {
            var data = await _userService.Download(id);

            return Ok(data.Message);
        }

        [HttpPost("[controller]/delete/{id}")]
        public async Task<IActionResult> Delete([FromBody] FileFilterModel model, string id)
        {
            var data = await _userService.DeleteFile(model, id);

            return Ok(data);
        }
    }
}
