using FileUpload.MVC.Models;
using FileUpload.MVC.Models.Categories;
using FileUpload.MVC.Models.Files;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UploadModel>> Upload(UploadFileDto fileDto);
        Task<Response<MyFilesViewModel>> GetMyFiles(FileFilterModel model);
        Task<Response<string>> GetDirectLink(string id);
        Task<Response<MyFileViewModel>> DeleteFile(FileFilterModel model, string id);
        Task<Response<List<GetCategoryDto>>> GetCategories();







    }
}
