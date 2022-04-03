using Amazon.S3;
using FileUpload.Upload.Application.Features.Commands.Files.Add;
using FileUpload.Upload.Application.Features.Commands.Files.Delete;
using FileUpload.Upload.Application.Features.Queries.Files.GetAll;
using FileUpload.Upload.Application.Features.Queries.Files.GetById;
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

namespace FileUpload.Upload.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMinioService _minioService;
        private readonly IHubContext<FileHub, IFileHub> _fileHub;
        public AmazonS3Client client { get; set; }

        public FileService(IMediator mediator,
            ISharedIdentityService sharedIdentityService,
            IHubContext<FileHub, IFileHub> fileHub, IMinioService minioService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;

            _fileHub = fileHub;
            _minioService = minioService;
        }

        public async Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model)
        {
            GetAllFilesQueryRequest query = new()
            {
                FilterModel = model,
                UserId = _sharedIdentityService.GetUserId
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<GetFileDto>> GetFileById(int id)
        {
            GetFileByIdQueryRequest query = new()
            {
                UserId = _sharedIdentityService.GetUserId,
                FileId = id
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<AddFileDto>> UploadAsync(IFormFile file, List<int> CategoriesId)
        {

            Response<AddFileDto> data = new();

            var ConnnnectionId = HubData.ClientsData.Where(x => x.UserId == "1").Select(x => x.ConnectionId).FirstOrDefault();

            if (!string.IsNullOrEmpty(ConnnnectionId))
            {
                await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);
            }

            var fileKey = await _minioService.Upload(file);

            if (!fileKey.IsSuccessful)
            {
                data = Response<AddFileDto>.Fail(fileKey.Errors.First(), 500);
            }
            else
            {
                Domain.Entities.File fileEntity = new()
                {
                    ApplicationUserId = _sharedIdentityService.GetUserId,
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

                var result = await _mediator.Send(command);

                if (result.IsSuccessful)
                {
                    data = Response<AddFileDto>.Success(new AddFileDto { FileId = fileEntity.Id, FileName = file.FileName }, 200);
                }
                else
                {
                    data = Response<AddFileDto>.Fail("veritabanına kayıt yapılırken hata oluştu", 500);
                }
            }

            if (!string.IsNullOrEmpty(ConnnnectionId))
            {
                await _fileHub.Clients.Client(ConnnnectionId).FilesUploaded(data);
            }

            return data;
        }

        public async Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, int id)
        {

            var fileKey = await GetFileKeyById(id);

            if (!fileKey.IsSuccessful)
            {
                return Response<FilePagerViewModel>.Fail(fileKey.Errors.First(), 500);
            }

            var result = await _minioService.Remove(fileKey.Value);

            if (!result.IsSuccessful)
            {
                return Response<FilePagerViewModel>.Fail(result.Errors.First());
            }

            DeleteFileCommand command = new()
            {
                UserId = _sharedIdentityService.GetUserId,
                FileId = id,
                FilterModel = model
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
                UserId = _sharedIdentityService.GetUserId,
                FileId = id
            };

            return await _mediator.Send(query);
        }
    }


}
