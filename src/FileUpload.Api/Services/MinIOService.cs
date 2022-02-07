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
using System.IO;
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

        public async Task<string> GetBucketName(string id)
        {
            if (!(await AmazonS3Util.DoesS3BucketExistAsync(client, _sharedIdentityService.GetUserId)))
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

        public async Task<Response<string>> UploadAsync(IFormFile file)
        {

            var key = string.Empty;
            try
            {
                key = Guid.NewGuid().ToString();
                var stream = file.OpenReadStream();
                var request = new PutObjectRequest()
                {
                    BucketName = await GetBucketName(_sharedIdentityService.GetUserId),
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
                return Response<string>.Fail(e.Message, 500);
            }
            return Response<string>.Success(key, 200);

          

        }
    }





}
