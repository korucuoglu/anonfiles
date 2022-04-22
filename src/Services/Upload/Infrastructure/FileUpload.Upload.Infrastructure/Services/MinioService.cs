using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUpload.Shared.Wrappers;
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
        public const string BucketName = "admin";

        public MinioService(IConfiguration configuration)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
                ServiceURL = configuration["MinioAccessInfo:EndPoint"],
                ForcePathStyle = true,
                SignatureVersion = "2"
            };
            client = new AmazonS3Client(configuration["MinioAccessInfo:AccessKey"], configuration["MinioAccessInfo:SecretKey"], config);
        }
        public async Task<bool> BucketExist(string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
        }
        public async Task<string> CreateBucket()
        {

            if (!await BucketExist(BucketName))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = BucketName,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(putBucketRequest);
            }

            return BucketName;

        }
        public async Task<Response<string>> Upload(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                PutObjectRequest putObjectRequest = new()
                {
                    BucketName = await CreateBucket(),
                    InputStream = stream,
                    AutoCloseStream = true,
                    Key = Guid.NewGuid().ToString(),
                    ContentType = file.ContentType
                };
                var encodedFilename = Uri.EscapeDataString(file.FileName);
                putObjectRequest.Metadata.Add("original-filename", encodedFilename);
                putObjectRequest.Headers.ContentDisposition = $"attachment; filename=\"{encodedFilename}\"";
                await client.PutObjectAsync(putObjectRequest);

                return Response<string>.Success(data: putObjectRequest.Key, 200);
            }

            catch (Exception e)
            {
                return Response<string>.Fail(e.Message, 500);
            }

        }
        public async Task<Response<NoContent>> Remove(string fileKey)
        {
            var result = await FileExist(BucketName, fileKey);

            if (!result.IsSuccessful)
            {
                return result;
            }
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = await CreateBucket(),
                    Key = fileKey
                };

                await client.DeleteObjectAsync(deleteObjectRequest);

                return Response<NoContent>.Success(200);
            }
            catch (Exception e)
            {
                return Response<NoContent>.Fail(e.Message, 500);
            }
        }
        public async Task<Response<NoContent>> Download(string fileKey)
        {
            var result = await FileExist(BucketName, fileKey);

            if (!result.IsSuccessful)
            {
                return result;
            }

            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = await CreateBucket(),
                    Key = fileKey,
                    Protocol = Protocol.HTTP,
                    Expires = DateTime.Now.AddMinutes(30)
                };
                var data = client.GetPreSignedURL(request);

                return Response<NoContent>.Success(message: data, 200);
            }

            catch (Exception e)
            {
                return Response<NoContent>.Fail(e.Message, 500);
            }
        }
        public async Task<Response<NoContent>> FileExist(string bucketName, string fileKey)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest()
            {
                BucketName = bucketName,
                Key = fileKey
            };

            try
            {
                await client.GetObjectMetadataAsync(request);
                return Response<NoContent>.Success(200);
            }
            catch (Exception e)
            {
                return Response<NoContent>.Fail(e.Message, 500);
            }
        }
    }
}

