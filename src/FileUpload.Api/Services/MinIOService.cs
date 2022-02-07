using FileUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{

    public class MinIOService
    {
        public Task<Response<bool>> DeleteAsyncByMetaId(string metaId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<string>> GetDirectLinkByMetaId(string metaId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<List<MyFilesViewModel>>> GetMyFiles()
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UploadModel>> UploadAsync(IFormFile file)
        {
            throw new System.NotImplementedException();
        }
    }





}
