﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Api.Hubs;
using FileUpload.Api.Models;
using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{

    public class MinIOService
    {
        private readonly IConfiguration configuration;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IRepository<Data.Entity.File> _fileRepository;
        private readonly IRepository<Data.Entity.UserInfo> _userInfoRepository;
        private readonly IHubContext<FileHub, IFileHub> _fileHub;
        private ILogger<MinIOService> Logger { get; set; }

        public AmazonS3Client client { get; set; }

        public MinIOService(IConfiguration configuration, ISharedIdentityService sharedIdentityService, ILogger<MinIOService> logger, IRepository<Data.Entity.File> fileRepository, IRepository<UserInfo> userInfoRepository, IHubContext<FileHub, IFileHub> fileHub)
        {
            this.configuration = configuration;
            _sharedIdentityService = sharedIdentityService;
            Logger = logger;
            _fileRepository = fileRepository;
            _userInfoRepository = userInfoRepository;
            _fileHub = fileHub;

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
                ServiceURL = configuration["MinioAccessInfo:EndPoint"],
                ForcePathStyle = true,
                SignatureVersion = "2"
            };
            client = new AmazonS3Client(configuration["MinioAccessInfo:AccessKey"], configuration["MinioAccessInfo:SecretKey"], config);

        }

        public async Task<string> GetBucketName()
        {
            if (!(await AmazonS3Util.DoesS3BucketExistV2Async(client, _sharedIdentityService.GetUserId)))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = _sharedIdentityService.GetUserId,
                    UseClientRegion = true
                };

                await client.PutBucketAsync(putBucketRequest);
            }

            return _sharedIdentityService.GetUserId;

        }

        public async Task<Response<UploadModel>> UploadAsync(UploadFileDto dto)
        {
            var key = string.Empty;

            var ConnnnectionId = HubData.ClientsData.Where(x => x.UserId == "1").Select(x => x.ConnectionId).FirstOrDefault();

            Response<UploadModel> data = new();

            foreach (var file in dto.Files)
            {
                try
                {

                    await _fileHub.Clients.Client(ConnnnectionId).FilesUploadStarting(file.FileName);

                    key = Guid.NewGuid().ToString();
                    var stream = file.OpenReadStream();
                    var request = new PutObjectRequest()
                    {
                        BucketName = await GetBucketName(),
                        InputStream = stream,
                        AutoCloseStream = true,
                        Key = $"{key}",
                        ContentType = file.ContentType
                    };
                    var encodedFilename = Uri.EscapeDataString(file.FileName);
                    request.Metadata.Add("original-filename", encodedFilename);
                    request.Headers.ContentDisposition = $"attachment; filename=\"{encodedFilename}\"";
                    await client.PutObjectAsync(request);

                    Data.Entity.File fileEntity = new()
                    {
                        ApplicationUserId = _sharedIdentityService.GetUserId,
                        FileName = file.FileName,
                        Size = file.Length,
                        Id = key,
                        Extension = Path.GetExtension(file.FileName).Replace(".", "").ToUpper()
                    };

                    (await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId)).UsedSpace += file.Length;
                    await _fileRepository.AddAsync(fileEntity);

                    data = Response<UploadModel>.Success(new UploadModel { FileId = key, FileName = file.FileName }, 200);

                }

                catch (Exception e)
                {
                    Logger.LogError("Error ocurred In UploadFileAsync", e.Message);
                    data = Response<UploadModel>.Fail(e.Message, 500);
                }

                finally
                {
                    await _fileHub.Clients.Client(ConnnnectionId).FilesUploaded(data);
                }
            }

            return Response<UploadModel>.Success(200);

           
        }

        public async Task<Response<MyFilesViewModel>> GetMyFiles(FileFilterModel model)
        {
            return await Filter.FilterFile(_fileRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId), model);
        }

        public async Task<Response<string>> Download(string key)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = _sharedIdentityService.GetUserId,
                Key = key,
                Protocol = Protocol.HTTP,
                Expires = DateTime.Now.AddMinutes(30)
            };
            var data = client.GetPreSignedURL(request);

            return await Task.FromResult(Response<string>.Success(data, 200));



        }

        public async Task<Response<MyFileViewModel>> Remove(FileFilterModel model, string key)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _sharedIdentityService.GetUserId,
                    Key = key
                };

                await client.DeleteObjectAsync(deleteObjectRequest);
                var file = await _fileRepository.FirstOrDefaultAsync(x => x.Id == key && x.ApplicationUserId == _sharedIdentityService.GetUserId);
                (await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId)).UsedSpace -= file.Size;

                var data = await Filter.GetOneFileAfterRemovedFile(_fileRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId), model);
                _fileRepository.Remove(file);
                return data;

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
