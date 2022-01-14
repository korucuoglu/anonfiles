using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

        //private async Task<MultipartContent> GetMultipartContentAsync(IFormFile[] files, string key)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        var multipartContent = new MultipartFormDataContent();

        //        foreach (var file in files)
        //        {
        //            await file.CopyToAsync(ms);
        //            multipartContent.Add(new ByteArrayContent(ms.ToArray()), key, file.FileName);
        //        }

        //        return multipartContent;
        //    };
        //}

        //public async Task<object> Upload(IFormFile[] files, string uri)
        //{
        //    var dto = new
        //    {
        //        message = new List<string>(),
        //    };

        //    var multipartContent = await GetMultipartContentAsync(files, "file");

        //    var response = await HttpClient.PostAsync($"{uri}", multipartContent);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        //        return data;
        //    }

        //    return "";
        //}

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

        public async Task<Response<List<string>>> Upload(IFormFile file, string uri)
        {
            
            var multipartContent = await GetMultipartContentAsync(file, "files");

            var response = await HttpClient.PostAsync($"{uri}", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                return Response<List<string>>.Success(data, 200);
            }

            return Response<List<string>>.Fail("Upload yapılırken hata meydana geldi", 500);
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
