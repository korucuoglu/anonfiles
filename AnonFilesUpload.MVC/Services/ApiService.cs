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
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
       
        public ApiService(HttpClient client)
        {
            _client = client;
            
        }

        //public async Task<string> GetAsync(string uri)
        //{
        //    var response = await _client.GetAsync(uri);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsStringAsync();
        //        return await Task.FromResult(data);
        //    }

        //    return await Task.FromResult("");
        //}

        public async Task<string> GetGenericAsync(string uri)
        {
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // dtos = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsStringAsync();
              
            }


            return "";
        }

        //public async Task<T> GetAllAsync<T>(string uri)
        //{
        //    T dtos = default(T);

        //    var response = await _client.GetAsync(uri);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        dtos = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        //    }

        //    return dtos;
        //}

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


        public async Task<HttpResponseMessage> UploadImageNew(string url, byte[] ImageData)
        {
            var requestContent = new MultipartFormDataContent();
            //    here you can specify boundary if you need---^
            var imageContent = new ByteArrayContent(ImageData);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "image", "image.jpg");

            return await _client.PostAsync(url, requestContent);
        }

       
    }
}
