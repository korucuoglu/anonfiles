using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FileUpload.MVC.Application.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileUpload.Shared.Dtos.Categories;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AddFileDto> Upload(FilesCategoriesDto fileDto);
        Task<FilesPagerViewModel> GetMyFiles(FileFilterModel model);
        Task<string> GetDirectLink(string id);
        Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id);
        Task<List<GetCategoryDto>> GetCategories();
        Task<GetCategoryDto> AddCategory(AddCategoryDto dto);







    }
}
