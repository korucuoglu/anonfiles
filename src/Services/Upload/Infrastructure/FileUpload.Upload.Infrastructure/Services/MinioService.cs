﻿using Amazon;
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
        public async Task<bool> BucketExist(string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
        }
        public async Task<string> CreateBucket()
        {
            string bucketName = _sharedIdentityService.GetUserName;

            if (!await BucketExist(bucketName))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(putBucketRequest);
            }

            return bucketName;

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

               return Response<string>.Success(data: putObjectRequest.Key);
            }

            catch (Exception e)
            {
               return Response<string>.Fail(e.Message);
            }

        }
        public async Task<Response<NoContent>> Remove(string fileKey)
        {
            bool result = await FileExist(_sharedIdentityService.GetUserName, fileKey);

            if (!result)
            {
                return Response<NoContent>.Fail("Böyle bir dosya bulunamadı");
            }
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = await CreateBucket(),
                    Key = fileKey
                };

                await client.DeleteObjectAsync(deleteObjectRequest);

                return Response<NoContent>.Success();
            }
            catch (Exception e)
            {
               return Response<NoContent>.Fail(e.Message);
            }
        }
        public async Task<Response<NoContent>> Download(string fileKey)
        {
            bool result = await FileExist(_sharedIdentityService.GetUserName, fileKey);

            if (!result)
            {
                return Response<NoContent>.Fail("Böyle bir dosya bulunamadı");
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
                return Response<NoContent>.Fail(e.Message);
            }
        }
        public async Task<bool> FileExist(string bucketName, string fileKey)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest()
            {
                BucketName = bucketName,
                Key = fileKey
            };

            try
            {
                await client.GetObjectMetadataAsync(request);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
