using FileUpload.MVC.Application.Dtos.Categories;
using FileUpload.MVC.Application.Extension;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FileUpload.Shared.Wrappers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

            var data = await _client.CustomPostAsync<List<Response<AddFileDto>>>("minio", content);

            return new AddFileDto();
        }

        public async Task<FilesPagerViewModel> GetMyFiles(FileFilterModel model)
        {
            var data = await _client.CustomPostAsync<Response<FilesPagerViewModel>, FileFilterModel>("minio/myfiles", model);

            return data.Value;
        }

        public async Task<Response<NoContent>> Download(string id)
        {
            return await _client.CustomGetAsync<Response<NoContent>>($"minio/download/{id}");
        }

        public async Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id)
        {
            return await _client.CustomPostAsync<Response<FilePagerViewModel>, FileFilterModel>($"minio/{id}", model);

        }

        public async Task<List<GetCategoryDto>> GetCategories()
        {
            var data = await _client.CustomGetAsync<Response<List<GetCategoryDto>>>("categories");

            return data.Value;

        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto dto)
        {
            var data = await _client.CustomPostAsync<Response<GetCategoryDto>, AddCategoryDto>("categories", dto);

            return data.Value;
        }
    }
}
