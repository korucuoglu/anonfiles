using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.MVC.Application.Dtos.Files;
using FileUpload.MVC.Application.Dtos.Files.Pager;
using FileUpload.MVC.Application.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AddFileDto> Upload(FilesCategoriesDto fileDto);
        Task<FilesPagerViewModel> GetMyFiles(FileFilterModel model);
        Task<string> GetDirectLink(string id);
        Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id);
        Task<List<GetCategoryDto>> GetCategories();







    }
}
