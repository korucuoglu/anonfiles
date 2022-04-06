using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Services
{
    public interface IMinioService
    {

        public Task<bool> BucketExist(string bucketName);
        public Task<Response<NoContent>> FileExist(string bucketName, string fileKey);
        public Task<string> CreateBucket();
        public Task<Response<string>> Upload(IFormFile file);
        public Task<Response<NoContent>> Remove(string fileKey);
        public Task<Response<NoContent>> Download(string fileKey);
    }
}
