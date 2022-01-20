using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<AjaxReturningModel>> Upload(IFormFile file);
        Task<Response<DataViewModel>> GetMyFiles();
        Task<Response<string>> GetDirectLink(string id);
      

       



    }
}
