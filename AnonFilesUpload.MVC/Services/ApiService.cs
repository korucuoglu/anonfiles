using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services
{
    public class ApiService : IApiService
    {
        public HttpClient HttpClient { get; set; }

        public async Task<string> GetAsync(string uri)
        {
            var response = await HttpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return await Task.FromResult(data);
            }

            return await Task.FromResult("");
        }

        public async Task<T> GetAllAsync<T>(string uri)
        {
            T dtos = default(T);

            var response = await HttpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                dtos = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }

            return dtos;
        }


        private async Task<MultipartContent> GetMultipartContentAsync(IFormFile file, string key)
        {
            using (var ms = new MemoryStream())
            {
                var multipartContent = new MultipartFormDataContent();

                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), key, file.FileName);


                return multipartContent;
            };
        }

        public async Task<Response<AjaxReturningModel>> Upload(IFormFile file, string uri)
        {

            var multipartContent = await GetMultipartContentAsync(file, "file");

            var response = await HttpClient.PostAsync($"{uri}", multipartContent);

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

            return await HttpClient.PostAsync(url, requestContent);
        }

        //public async Task<bool> Remove(int id)
        //{
        //    var response = await HttpClient.DeleteAsync($"{Method}/{id}");

        //	if (response.IsSuccessStatusCode)
        //	{
        //		return true;
        //	}

        //	return false;
        //}



        //public async Task<bool> Update(T dto)
        //{
        //	var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        //	var response = await HttpClient.PutAsync($"{Method}", stringContent);

        //	if (response.IsSuccessStatusCode)
        //	{
        //		return true;
        //	}

        //	return false;
        //}
    }
}
