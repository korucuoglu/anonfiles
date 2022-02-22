using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.MVC.Services.Interfaces;
using FileUpload.MVC.Application.Wrappers;
using FileUpload.MVC.Application.Dtos.Files;
using FileUpload.MVC.Application.Dtos.Files.Pager;
using FileUpload.MVC.Application.Dtos.Categories;

namespace FileUpload.MVC.Infrastructure.Services
{
    public class UserService : IUserService
    {

        private readonly IApiService _apiService;

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Response<AddFileDto>> Upload(FilesCategoriesDto fileDto)
        {
            var content = await Helper.Helper.GetMultipartContentAsync(fileDto);

            var deserializeData = await _apiService.PostAsync("minio", content);

            var serializeData = JsonConvert.DeserializeObject<Response<AddFileDto>>(deserializeData);

            return serializeData;


        }

        public async Task<Response<FilesPagerViewModel>> GetMyFiles(FileFilterModel model)
        {
            string queryString = Helper.Helper.GetQueryString(model);

            var deserializeData = await _apiService.GetAsync($"minio/myfiles?{queryString}");
            var serializeData = JsonConvert.DeserializeObject<Response<FilesPagerViewModel>>(deserializeData);

            return serializeData;

        }

        public async Task<Response<string>> GetDirectLink(string id)
        {
            var deserializeData = await _apiService.GetAsync($"minio/download/{id}");

            var serializeData = JsonConvert.DeserializeObject<Response<string>>(deserializeData);

            return serializeData;
        }

        public async Task<Response<FilePagerViewModel>> DeleteFile(FileFilterModel model, string id)
        {
            string queryString = Helper.Helper.GetQueryString(model);

            var deserializeData = await _apiService.DeleteAsync($"minio/{id}?{queryString}");

            var serializeData = JsonConvert.DeserializeObject<Response<FilePagerViewModel>>(deserializeData);

            return serializeData;
        }

        public async Task<Response<List<GetCategoryDto>>> GetCategories()
        {
            var deserializeData = await _apiService.GetAsync("categories");
            var serializeData = JsonConvert.DeserializeObject<Response<List<GetCategoryDto>>>(deserializeData);

            return serializeData;
        }
    }
}
