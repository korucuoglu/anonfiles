using FileUpload.Upload.Application.Features.Commands.Files;
using FileUpload.Upload.Application.Interfaces.Hub;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Infrastructure.Hub;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Features.Queries.Files;

namespace FileUpload.Upload.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMediator _mediator;
        private readonly IMinioService _minioService;
        private readonly IHubContext<FileHub, IFileHub> _fileHub;

        public FileService(IMediator mediator,
            IHubContext<FileHub, IFileHub> fileHub, IMinioService minioService)
        {
            _mediator = mediator;
            _fileHub = fileHub;
            _minioService = minioService;
        }

        public async Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model)
        {
            GetAllFilesQueryRequest query = new()
            {
                FilterModel = model,
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<GetFileDto>> GetFileById(int id)
        {
            GetFileByIdQueryRequest query = new()
            {
                FileId = id
            };

            return await _mediator.Send(query);
        }
        public async Task<Response<AddFileDto>> UploadAsync(IFormFile file, List<int> CategoriesId)
        {
           
            var ConnnnectionId = HubData.ClientsData.Where(x => x.UserId == "1").Select(x => x.ConnectionId).FirstOrDefault();

            if (!string.IsNullOrEmpty(ConnnnectionId))
            {
                await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);
            }

            async Task<Response<AddFileDto>> GetResult()
            {
                var fileKey = await _minioService.Upload(file);

                if (!fileKey.IsSuccessful)
                {
                    return Response<AddFileDto>.Fail(fileKey.Errors.First(), 500);
                }

                Domain.Entities.File fileEntity = new()
                {
                    FileName = file.FileName,
                    Size = file.Length,
                    Extension = Path.GetExtension(file.FileName).Replace(".", "").ToUpper(),
                    FileKey = fileKey.Value,
                };

                if (CategoriesId.Any())
                {
                    CategoriesId.ForEach(x =>
                    {
                        fileEntity.FilesCategories.Add(new FileCategory() { CategoryId = x });
                    });
                }

                AddFileCommand command = new()
                {
                    File = fileEntity,
                };

                return await _mediator.Send(command);
            }

            var result = await GetResult();

            if (!string.IsNullOrEmpty(ConnnnectionId))
            {
                await _fileHub.Clients.Client(ConnnnectionId).FilesUploaded(result);
            }

            return result;
        }

        public async Task<Response<NoContent>> Remove(int id)
        {

            var fileKey = await GetFileKeyById(id);

            if (!fileKey.IsSuccessful)
            {
                return Response<NoContent>.Fail(fileKey.Errors.First(), 500);
            }

            var result = await _minioService.Remove(fileKey.Value);

            if (!result.IsSuccessful)
            {
                return Response<NoContent>.Fail(result.Errors.First(), 500);
            }

            DeleteFileCommand command = new()
            {
                FileId = id,
            };

            return await _mediator.Send(command);
        }

        public async Task<Response<NoContent>> Download(int id)
        {
            var fileKey = await GetFileKeyById(id);

            if (!fileKey.IsSuccessful)
            {
                return Response<NoContent>.Fail(fileKey.Errors.First(), 500);
            }

            var data = await _minioService.Download(fileKey.Value);

            return await Task.FromResult(data);
        }

        public async Task<Response<string>> GetFileKeyById(int id)
        {
            GetFileKeyById query = new()
            {
                FileId = id
            };

            return await _mediator.Send(query);
        }
    }


}
