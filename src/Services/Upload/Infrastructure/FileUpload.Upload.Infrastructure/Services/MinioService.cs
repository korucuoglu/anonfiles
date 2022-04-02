using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Upload.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FileUpload.Upload.Infrastructure.Services
{
    internal class MinioService : IMinioService
    {
        public AmazonS3Client client { get; set; }
        public ISharedIdentityService _sharedIdentityService;

        public MinioService(IConfiguration configuration, ISharedIdentityService sharedIdentityService = null)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
                ServiceURL = configuration["MinioAccessInfo:EndPoint"],
                ForcePathStyle = true,
                SignatureVersion = "2"
            };
            client = new AmazonS3Client(configuration["MinioAccessInfo:AccessKey"], configuration["MinioAccessInfo:SecretKey"], config);
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task CreateBucket(string bucketName)
        {

            if (!await BucketExist(bucketName))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(putBucketRequest);
            }
        }
        public async Task<bool> BucketExist(string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
        }

        public async Task<bool> Upload(IFormFile file)
        {
            bool result = false;

            try
            {
                var guid = Guid.NewGuid().ToString();
                await CreateBucket(guid);

                var stream = file.OpenReadStream();
                PutObjectRequest putObjectRequest = new()
                {
                    BucketName = guid,
                    InputStream = stream,
                    AutoCloseStream = true,
                    Key = Guid.NewGuid().ToString(),
                    ContentType = file.ContentType
                };
                var encodedFilename = Uri.EscapeDataString(file.FileName);
                putObjectRequest.Metadata.Add("original-filename", encodedFilename);
                putObjectRequest.Headers.ContentDisposition = $"attachment; filename=\"{encodedFilename}\"";
                await client.PutObjectAsync(putObjectRequest);

                result = true;
            }

            catch
            {
                result = false;
            }

            return await Task.FromResult(result);


        }

        public async Task Remove(string fileId)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _sharedIdentityService.GetUserId.ToString(),
                Key = fileId.ToString()
            };

            await client.DeleteObjectAsync(deleteObjectRequest);
        }

        public string Download(string fileId)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = _sharedIdentityService.GetUserId.ToString(),
                Key = fileId,
                Protocol = Protocol.HTTP,
                Expires = DateTime.Now.AddMinutes(30)
            };
            return client.GetPreSignedURL(request);
        }
    }
}

