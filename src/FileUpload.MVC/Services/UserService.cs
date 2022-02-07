using FileUpload.Shared.Models;
using FileUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FileUpload.MVC.Services
{
    public class UserService : IUserService
    {

        private readonly IApiService _apiService;

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Response<UploadModel>> Upload(IFormFile file)
        {
            var content = await Helper.GetMultipartContentAsync(file);

            var deserializeData = await _apiService.PostAsync("data", content);

            var serializeData = JsonConvert.DeserializeObject<Response<UploadModel>>(deserializeData);

            return serializeData;


        }

        public async Task<Response<List<MyFilesViewModel>>> GetMyFiles()
        {
            var deserializeData = await _apiService.GetAsync("data/myfiles");
            var serializeData = JsonConvert.DeserializeObject<Response<List<MyFilesViewModel>>>(deserializeData);

            return serializeData;

        }

        public async Task<Response<string>> GetDirectLink(string id)
        {
            var deserializeData = await _apiService.GetAsync($"data/{id}");

            var serializeData = JsonConvert.DeserializeObject<Response<string>>(deserializeData);

            return serializeData;
        }

        public async Task<Response<bool>> DeleteFile(string id)
        {
            var deserializeData = await _apiService.DeleteAsync($"data/{id}");

            var serializeData = JsonConvert.DeserializeObject<Response<bool>>(deserializeData);

            return serializeData;
        }

    }
}
