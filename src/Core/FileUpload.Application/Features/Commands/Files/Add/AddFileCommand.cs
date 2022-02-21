using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<UploadModel>>
    {
        public IFormFile[] Files { get; set; }

        public List<GetCategoryDto> Categories { get; set; }
    }
    public class AddCategoryCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(x => x.Files).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddFileCommand, Response<UploadModel>>
    {
        private readonly IRepository<Domain.Entities.File> _fileRepository;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<FileCategory> _filesCategoriesRepository;
        private readonly IConfiguration configuration;
        public AmazonS3Client client { get; set; }

        public AddCategoryCommandHandler(IRepository<Domain.Entities.File> fileRepository,
            ISharedIdentityService sharedIdentityService,
            IRepository<UserInfo> userInfoRepository,
            IRepository<FileCategory> filesCategoriesRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _sharedIdentityService = sharedIdentityService;
            _userInfoRepository = userInfoRepository;
            _filesCategoriesRepository = filesCategoriesRepository;
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

        public async Task<Response<UploadModel>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            Guid fileId;

            Response<UploadModel> data = new();

            async Task<string> GetBucketName()
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

            foreach (var file in request.Files)
            {
                try
                {

                    fileId = Guid.NewGuid();
                    var stream = file.OpenReadStream();
                    PutObjectRequest putObjectRequest = new ()
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

                    (await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId)).UsedSpace += file.Length;
                    await _fileRepository.AddAsync(fileEntity);

                    foreach (var category in request.Categories)
                    {
                        FileCategory file_category = new()
                        {
                            CategoryId = category.Id,
                            FileId = fileId
                        };

                        await _filesCategoriesRepository.AddAsync(file_category);
                    }

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
    }
}
