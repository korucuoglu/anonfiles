using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{
    public interface IFileService
    {
        public Task<Response<UploadModel>> UploadAsync(IFormFile file);
        public Task<Response<DataViewModel>> GetFilesByUserId();
        public Task<Response<string>> GetDirectLinkByMetaId(string metaId);
        public Task<Response<bool>> DeleteAsyncByMetaId(string metaId);

    }
}
