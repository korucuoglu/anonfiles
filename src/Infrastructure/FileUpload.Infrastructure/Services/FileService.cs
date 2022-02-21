﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Features.Commands.Files.Add;
using FileUpload.Application.Features.Commands.Files.Delete;
using FileUpload.Application.Features.Queries.Files.GetAll;
using FileUpload.Application.Features.Queries.Files.GetById;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IConfiguration configuration;
        public AmazonS3Client client { get; set; }

        public FileService(IMediator mediator, ISharedIdentityService sharedIdentityService, IConfiguration configuration)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
            this.configuration = configuration;

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
                ServiceURL = configuration["MinioAccessInfo:EndPoint"],
                ForcePathStyle = true,
                SignatureVersion = "2"
            };
            client = new AmazonS3Client(configuration["MinioAccessInfo:AccessKey"], configuration["MinioAccessInfo:SecretKey"], config);
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

        public async Task<string> GetBucketName()
        {
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(client, _sharedIdentityService.GetUserId.ToString()))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = _sharedIdentityService.GetUserId.ToString(),
                    UseClientRegion = true
                };

                await client.PutBucketAsync(putBucketRequest);
            }

            return _sharedIdentityService.GetUserId.ToString();

        }

        public async Task<Response<UploadModel>> UploadAsync(IFormFile[] files)
        {
            Guid fileId;

            Response<UploadModel> data = new();

            foreach (var file in files)
            {
                try
                {
                    // await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);

                    fileId = Guid.NewGuid();
                    var stream = file.OpenReadStream();
                    PutObjectRequest putObjectRequest = new()
                    {
                        BucketName = await GetBucketName(),
                        InputStream = stream,
                        AutoCloseStream = true,
                        Key = $"{fileId}",
                        ContentType = file.ContentType
                    };
                    var encodedFilename = Uri.EscapeDataString(file.FileName);
                    putObjectRequest.Metadata.Add("original-filename", encodedFilename);
                    putObjectRequest.Headers.ContentDisposition = $"attachment; filename=\"{encodedFilename}\"";
                    await client.PutObjectAsync(putObjectRequest);

                    Domain.Entities.File fileEntity = new()
                    {
                        ApplicationUserId = _sharedIdentityService.GetUserId,
                        FileName = file.FileName,
                        Size = file.Length,
                        Id = fileId,
                        Extension = Path.GetExtension(file.FileName).Replace(".", "").ToUpper(),

                    };

                    AddFileCommand command = new() { File = fileEntity };

                    await _mediator.Send(command);

                    data = Response<UploadModel>.Success(new UploadModel { FileId = fileId, FileName = file.FileName }, 200);

                }

                catch (Exception e)
                {
                    data = Response<UploadModel>.Fail(e.Message, 500);
                }

                finally
                {
                    // await _fileHub.Clients.Client(ConnnnectionId).FilesUploaded(data);

                }
            }

            return Response<UploadModel>.Success(200);
        }

        public async Task<Response<MyFileViewModel>> Remove(FileFilterModel model, Guid fileId)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _sharedIdentityService.GetUserId.ToString(),
                    Key = fileId.ToString()
                };

                await client.DeleteObjectAsync(deleteObjectRequest);

                DeleteFileCommand command = new()
                {
                    UserId = _sharedIdentityService.GetUserId,
                    FileId = fileId,
                    FilterModel = model
                };

                return await _mediator.Send(command);

                //var file = await _fileRepository.FirstOrDefaultAsync(x => x.Id == fileId && x.ApplicationUserId == _sharedIdentityService.GetUserId);
                //(await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId)).UsedSpace -= file.Size;

                //var data = await Filter.GetOneFileAfterRemovedFile(_fileRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId), model);
                //_fileRepository.Remove(file);
                //return data;

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<MyFileViewModel>.Fail(e.Message, 500);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<MyFileViewModel>.Fail(e.Message, 500);
            }
        }
    }


}
