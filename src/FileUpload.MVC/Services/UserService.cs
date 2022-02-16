using FileUpload.Shared.Models;
using FileUpload.MVC.Services.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileUpload.Shared.Models.Files;
using FileUpload.Shared.Helper;
using FileUpload.Shared.Dtos.Categories;

namespace FileUpload.MVC.Services
{
    public class UserService : IUserService
    {

        private readonly IApiService _apiService;

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Response<UploadModel>> Upload(UploadFileDto fileDto)
        {
            var content = await Helper.GetMultipartContentAsync(fileDto);

            var deserializeData = await _apiService.PostAsync("minio", content);

            var serializeData = JsonConvert.DeserializeObject<Response<UploadModel>>(deserializeData);

            return serializeData;


        }

        public async Task<Response<MyFilesViewModel>> GetMyFiles(FileFilterModel model)
        {
            string queryString = Helper.GetQueryString(model);

            var deserializeData = await _apiService.GetAsync($"minio/myfiles?{queryString}");
            var serializeData = JsonConvert.DeserializeObject<Response<MyFilesViewModel>>(deserializeData);

            return serializeData;

        }

        public async Task<Response<string>> GetDirectLink(string id)
        {
            var deserializeData = await _apiService.GetAsync($"minio/download/{id}");

            var serializeData = JsonConvert.DeserializeObject<Response<string>>(deserializeData);

            return serializeData;
        }

        public async Task<Response<MyFileViewModel>> DeleteFile(FileFilterModel model, string id)
        {
            string queryString = Helper.GetQueryString(model);

            var deserializeData = await _apiService.DeleteAsync($"minio/{id}?{queryString}");

            var serializeData = JsonConvert.DeserializeObject<Response<MyFileViewModel>>(deserializeData);

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
