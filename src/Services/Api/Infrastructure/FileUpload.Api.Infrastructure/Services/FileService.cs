using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using AutoMapper;
using FileUpload.Application.Features.Commands.Files.Add;
using FileUpload.Application.Features.Commands.Files.Delete;
using FileUpload.Application.Features.Queries.Files.GetAll;
using FileUpload.Application.Features.Queries.Files.GetById;
using FileUpload.Application.Interfaces.Hub;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Domain.Entities;
using FileUpload.Infrastructure.Hub;
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

namespace FileUpload.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IConfiguration configuration;
        private readonly IHubContext<FileHub, IFileHub> _fileHub;
        private readonly IMapper _mapper;
        public AmazonS3Client client { get; set; }

        public FileService(IMediator mediator,
            ISharedIdentityService sharedIdentityService,
            IConfiguration configuration,
            IHubContext<FileHub, IFileHub> fileHub, IMapper mapper)
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
            _fileHub = fileHub;
            _mapper = mapper;
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

        public async Task<Response<GetFileDto>> GetFileById(Guid id)
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

        public async Task<Response<AddFileDto>> UploadAsync(IFormFile[] files, List<Guid> CategoriesId)
        {
            Guid fileId;

            Response<AddFileDto> data = new();

            var ConnnnectionId = HubData.ClientsData.Where(x => x.UserId == "1").Select(x => x.ConnectionId).FirstOrDefault();

            List<Domain.Entities.File> fileListEntity = new();

            foreach (var file in files)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ConnnnectionId))
                    {
                        await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);
                    }


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
                        Id = fileId,
                        ApplicationUserId = _sharedIdentityService.GetUserId,
                        FileName = file.FileName,
                        Size = file.Length,
                        Extension = Path.GetExtension(file.FileName).Replace(".", "").ToUpper(),
                        
                    };

                    CategoriesId.ForEach(x =>
                    {
                        fileEntity.FilesCategories.Add(new FileCategory() { CategoryId = x });
                    });

                    fileListEntity.Add(fileEntity);


                    data = Response<AddFileDto>.Success(new AddFileDto { FileId = fileId, FileName = file.FileName }, 200);

                }

                catch (Exception e)
                {
                    data = Response<AddFileDto>.Fail(e.Message, 500);
                }

                finally
                {
                    if (!string.IsNullOrEmpty(ConnnnectionId))
                    {
                        await _fileHub.Clients.Client(ConnnnectionId).FilesUploaded(data);
                    }
                }
            }

            AddFileCommand command = new()
            {
                Files = fileListEntity,
                AplicationUserId = _sharedIdentityService.GetUserId
            };

            await _mediator.Send(command);

            return Response<AddFileDto>.Success(200);
        }

        public async Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, Guid fileId)
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

        public async Task<Response<string>> Download(string id)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = _sharedIdentityService.GetUserId.ToString(),
                Key = id,
                Protocol = Protocol.HTTP,
                Expires = DateTime.Now.AddMinutes(30)
            };
            var data = client.GetPreSignedURL(request);

            return await Task.FromResult(Response<string>.Success(data, 200));
        }
    }


}
