using AnonFilesUpload.Shared.Models;
using AnonFilesUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services
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
        
        public async Task<Response<DataViewModel>> GetMyFiles()
        {
            var deserializeData = await _apiService.GetAsync("data/myfiles");
            var serializeData = JsonConvert.DeserializeObject<Response<DataViewModel>>(deserializeData);

            return serializeData;

        }

        public async Task<Response<string>> GetDirectLink(string id)
        {
            var deserializeData = await _apiService.GetAsync($"data/getdirect/{id}");

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
