using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FileUpload.Shared.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AddFileDto> Upload(FilesCategoriesDto fileDto);
        Task<FilesPagerViewModel> GetMyFiles(FileFilterModel model);
        Task<Response<NoContent>> Download(string id);
        Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id);
        Task<List<GetCategoryDto>> GetCategories();
        Task<GetCategoryDto> AddCategory(AddCategoryDto dto);







    }
}
