using Newtonsoft.Json;
using System.Collections.Generic;
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

        //public async Task<T> AddAsync(T dto)
        //{
        //	var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        //	var response = await HttpClient.PostAsync($"{Method}", stringContent);

        //	if (response.IsSuccessStatusCode)
        //	{
        //		dto = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        //		return dto;
        //	}

        //	return default;
        //}

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
