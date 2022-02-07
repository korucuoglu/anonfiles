using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Shared.Models;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Http;
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
        private ILogger<MinIOService> Logger { get; set; }

        public AmazonS3Client client { get; set; }

        public MinIOService(IConfiguration configuration, ISharedIdentityService sharedIdentityService, ILogger<MinIOService> logger)
        {
            this.configuration = configuration;
            _sharedIdentityService = sharedIdentityService;
            Logger = logger;

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

        public async Task<Response<UploadModel>> UploadAsync(IFormFile file)
        {

            var key = string.Empty;
            try
            {
                key = Guid.NewGuid().ToString();
                var stream = file.OpenReadStream();
                var request = new PutObjectRequest()
                {
                    BucketName = await GetBucketName(),
                    InputStream = stream,
                    AutoCloseStream = true,
                    Key = key,
                    ContentType = file.ContentType
                };
                var encodedFilename = Uri.EscapeDataString(file.FileName);
                request.Metadata.Add("original-filename", encodedFilename);
                request.Headers.ContentDisposition = $"attachment; filename=\"{encodedFilename}\"";
                await client.PutObjectAsync(request);
            }
            catch (Exception e)
            {
                Logger.LogError("Error ocurred In UploadFileAsync", e);
                return Response<UploadModel>.Fail(e.Message, 500);
            }
            return Response<UploadModel>.Success(new UploadModel { FileId = key, FileName = file.FileName}, 200);

        }

        public async Task<Response<List<MyFilesViewModel>>> GetMyFiles()
        {

            if (await AmazonS3Util.DoesS3BucketExistV2Async(client, _sharedIdentityService.GetUserId))
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = _sharedIdentityService.GetUserId
                };

                var fileList = (await client.ListObjectsAsync(request)).S3Objects;

                var model = new List<MyFilesViewModel>();

                foreach (var item in fileList)
                {
                    model.Add(new MyFilesViewModel { FileId = item.Key, FileName = item.Key });
                }

                return Response<List<MyFilesViewModel>>.Success(model, 200);
            }

            return Response<List<MyFilesViewModel>>.Success(200);

        }

        public async Task<Response<string>> Download(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            var data = client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = _sharedIdentityService.GetUserId,
                Key = key,
                Expires = DateTime.Now.AddMinutes(30)
            });

            return Response<string>.Success(data, 200);


           
        }

        public async Task<Response<bool>> Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return Response<bool>.Fail(false, 404);

            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _sharedIdentityService.GetUserId,
                    Key = key
                };

                await client.DeleteObjectAsync(deleteObjectRequest);
                return Response<bool>.Success(true, 200);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<bool>.Fail(e.Message, 500);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
                return Response<bool>.Fail(e.Message, 500);
            }



        }
    }


}
