using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using AutoMapper;
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
using Microsoft.Extensions.Configuration;
using System;
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

        public async Task<Response<AddFileDto>> UploadAsync(IFormFile[] files, List<int> CategoriesId)
        {

            Response<AddFileDto> data = new();

            var ConnnnectionId = HubData.ClientsData.Where(x => x.UserId == "1").Select(x => x.ConnectionId).FirstOrDefault();

            foreach (var file in files)
            {

                if (!string.IsNullOrEmpty(ConnnnectionId))
                {
                    await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);
                }

                bool result = await _minioService.Upload(file);

                if (!result)
                {
                    continue;
                }

                Domain.Entities.File fileEntity = new()
                {
                    ApplicationUserId = _sharedIdentityService.GetUserId,
                    FileName = file.FileName,
                    Size = file.Length,
                    Extension = Path.GetExtension(file.FileName).Replace(".", "").ToUpper(),
                };

                AddFileCommand command = new()
                {
                    File = fileEntity,
                    AplicationUserId = _sharedIdentityService.GetUserId
                };

                await _mediator.Send(command);

                if (CategoriesId.Any())
                {
                    CategoriesId.ForEach(x =>
                    {
                        fileEntity.FilesCategories.Add(new FileCategory() { CategoryId = x });
                    });
                }

                data = Response<AddFileDto>.Success(new AddFileDto { FileId = fileEntity.Id, FileName = file.FileName }, 200);
            }



           

            return Response<AddFileDto>.Success(200);
        }

        public async Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, int fileId)
        {
            try
            {
                await _minioService.Remove(fileId.ToString());

                DeleteFileCommand command = new()
                {
                    UserId = _sharedIdentityService.GetUserId,
                    FileId = fileId,
                    FilterModel = model
                };

                return await _mediator.Send(command);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<FilePagerViewModel>.Fail(e.Message, 500);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<FilePagerViewModel>.Fail(e.Message, 500);
            }
        }

        public async Task<Response<NoContent>> Download(string id)
        {
            var data = _minioService.Download(id);

            return await Task.FromResult(Response<NoContent>.Success(message: data, statusCode: 200));
        }
    }


}
