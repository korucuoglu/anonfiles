using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{
    public interface IFileService
    {
        public Task<Response<UploadModel>> UploadAsync(IFormFile file);
        public Task<Response<List<MyFilesViewModel>>> GetMyFiles();
        public Task<Response<string>> GetDirectLinkByMetaId(string metaId);
        public Task<Response<bool>> DeleteAsyncByMetaId(string metaId);

    }
}
