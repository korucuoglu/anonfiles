using AnonFilesUpload.MVC.Services.Interfaces;
using System.Net.Http;
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

        public async Task<string> DeleteAsync(string uri)
        {
            var response = await _client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }

            return "";
        }

        public async Task<string> GetAsync(string uri)
        {
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }

            return "";
        }

        public async Task<string> PostAsync(string uri, HttpContent content)
        {
            var response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }

            return "";
        }

        public async Task<string> PutAsync(string uri, HttpContent content)
        {
            var response = await _client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }

            return "";
        }
    }
}
