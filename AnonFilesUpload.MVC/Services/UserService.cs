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

        private async Task<MultipartContent> GetMultipartContentAsync(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                var multipartContent = new MultipartFormDataContent();

                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "file", file.FileName);


                return multipartContent;
            };
        }

        public async Task<Response<AjaxReturningModel>> Upload(IFormFile file)
        {
            var multipartContent = await GetMultipartContentAsync(file);

            var deserializeData = await _apiService.PostAsync("data", multipartContent);

            var serializeData = JsonConvert.DeserializeObject<AjaxReturningModel>(deserializeData);

            return Response<AjaxReturningModel>.Success(serializeData, 200);
        }
        
        public async Task<Response<DataViewModel>> GetMyFiles()
        {
            var deserializeData = await _apiService.GetAsync("data/myfiles");
            var serializeData = JsonConvert.DeserializeObject<DataViewModel>(deserializeData);

            return Response<DataViewModel>.Success(serializeData, 200);

        }

        public async Task<Response<string>> GetDirectLink(string id)
        {
            var deserializeData = await _apiService.GetAsync($"data/getdirect/{id}");

            return Response<string>.Success(deserializeData, 200);
        }
    }
}
