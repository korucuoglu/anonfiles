using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Services
{
    public interface IMinioService
    {

        public Task CreateBucket(string bucketName);

        public Task<bool> Upload(IFormFile file);

        public Task<bool> BucketExist(string bucketName);

        public Task Remove(string fileId);
        public string Download(string fileId);
    }
}
