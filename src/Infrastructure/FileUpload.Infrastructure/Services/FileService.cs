using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Features.Commands.Files.Add;
using FileUpload.Application.Features.Queries.Files.GetAll;
using FileUpload.Application.Features.Queries.Files.GetById;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public FileService(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<MyFilesViewModel>> GetAllFiles(FileFilterModel model)
        {
            GetAllFilesQueryRequest query = new()
            {
                FilterModel = model,
                UserId = _sharedIdentityService.GetUserId
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<FileDto>> GetFileById(Guid id)
        {
            GetFileByIdQueryRequest query = new()
            {
                UserId = _sharedIdentityService.GetUserId,
                FileId = id
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<UploadModel>> UploadAsync(AddFileCommand dto)
        {
            return await _mediator.Send(dto);
        }
    }

   
}
