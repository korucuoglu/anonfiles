using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UploadModel>> Upload(IFormFile file);
        Task<Response<List<MyFilesViewModel>>> GetMyFiles();
        Task<Response<string>> GetDirectLink(string id);
        Task<Response<bool>> DeleteFile(string id);
      

       



    }
}
