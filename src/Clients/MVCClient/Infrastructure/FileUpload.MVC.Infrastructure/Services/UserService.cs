using System.Threading.Tasks;
using System.Collections.Generic;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.MVC.Application.Dtos.Categories;
using System.Net.Http;
using FileUpload.MVC.Application.Extension;
using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FileUpload.Shared.Dtos.Categories;

namespace FileUpload.MVC.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        
        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<AddFileDto> Upload(FilesCategoriesDto fileDto)
        {
            var content = await Helper.Helper.GetMultipartContentAsync(fileDto);

            return await _client.CustomPostAsyncWithHttpContent<AddFileDto>("minio", content, true);
        }

        public async Task<FilesPagerViewModel> GetMyFiles(FileFilterModel model)
        {
            return await _client.CustomPostAsync<FilesPagerViewModel, FileFilterModel>("minio/myfiles", model, true);

        }

        public async Task<string> GetDirectLink(string id)
        {
            return await _client.CustomGetAsync<string>($"minio/download/{id}", true);
        }

        public async Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id)
        {
            return await _client.CustomPostAsyncReturnsResponse<FilePagerViewModel, FileFilterModel>($"minio/{id}", model, true);

        }

        public async Task<List<GetCategoryDto>> GetCategories()
        {
            return await _client.CustomGetAsync<List<GetCategoryDto>>("categories", true);
         
        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto dto)
        {
            return await _client.CustomPostAsync<GetCategoryDto, AddCategoryDto>("categories", dto, true);
        }
    }
}
