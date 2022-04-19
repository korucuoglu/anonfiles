using FileUpload.Shared.Base;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Features.Commands.Files;
using FileUpload.Upload.Application.Features.Queries.Files;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Upload.Controllers
{

    public class FilesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files)
        {
            List<Response<NoContent>> result = new();

            foreach (var file in files)
            {
                var data = await _mediator.Send(new AddFileCommand() { File = file });

                result.Add(data);
            }
            return Ok(result);
        }

        [HttpPost("myfiles")]
        public async Task<IActionResult> GetAllAsync(FileFilterModel model)
        {
            var data = await _mediator.Send(new GetAllFilesQueryRequest() { FilterModel = new(model) });
            return Result(data);
        }

        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        [HttpGet("myfiles/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var data = await _mediator.Send(new GetFileByIdQueryRequest() { FileId = id });
            return Result(data);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _mediator.Send(new DeleteFileCommand() { FileId = id });
            return Result(data);
        }

        [HttpGet("download/{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Download(string id)
        {
            var data = await _mediator.Send(new GetDownloadLink() { FileId = id });
            return Result(data);
        }
    }
}
