using FileUpload.Shared.Base;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Services;
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
        private readonly IHashService _hashService;
        private readonly IMediator _mediator;

        public FilesController( IHashService hashService, IMediator mediator)
        {
            _hashService = hashService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files)
        {
            List<Response<NoContent>> result = new();

            foreach (var file in files)
            {
                var data = await _mediator.Send(new AddFileCommand() { File = file});

                result.Add(data);
            }
            return Ok(result);

        }

        [HttpPost("myfiles")]
        public async Task<IActionResult> GetAllAsync(FileFilterModel model)
        {
            GetAllFilesQueryRequest query = new()
            {
                FilterModel = new(model)
            };
            var data = await _mediator.Send(query);

            return Result(data);

        }

        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        [HttpGet("myfiles/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {

            GetFileByIdQueryRequest query = new()
            {
                FileId = _hashService.Decode(id)
            };

            var data = await _mediator.Send(query);

            return Result(data);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Remove(string id)
        {
            DeleteFileCommand command = new()
            {
                FileId = _hashService.Decode(id)
            };

            var data = await _mediator.Send(command);

            return Result(data);
        }

        [HttpGet("download/{id}")]
        [ServiceFilter(typeof(NotFoundFilterAttribute<File>))]
        public async Task<IActionResult> Download(string id)
        {
            GetDownloadLink query = new()
            {
                FileId = _hashService.Decode(id)
            };

            var data = await _mediator.Send(query);

            return Result(data);
        }
    }
}
