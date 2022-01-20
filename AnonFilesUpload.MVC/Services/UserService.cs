using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Data.Models;
using AnonFilesUpload.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
       
        public UserService(HttpClient client)
        {
            _client = client;
            
        }

        public async Task<string> GetGenericAsync(string uri)
        {
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
              
            }


            return "";
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

        public async Task<Response<AjaxReturningModel>> Upload(IFormFile file, string uri)
        {
            var multipartContent = await GetMultipartContentAsync(file);

            var response = await _client.PostAsync($"{uri}", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<AjaxReturningModel>(await response.Content.ReadAsStringAsync());

                return Response<AjaxReturningModel>.Success(data, 200);
            }

            return Response<AjaxReturningModel>.Fail("Upload yapılırken hata meydana geldi", 500);
        }

        public async Task<string> UploadTest(IFormFile file)
        {
            Thread.Sleep(1000);
            return await Task.FromResult(file.FileName);
        }

       
    }
}
