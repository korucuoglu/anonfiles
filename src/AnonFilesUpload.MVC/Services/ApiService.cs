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
            return await response.Content.ReadAsStringAsync();



        }

        public async Task<string> GetAsync(string uri)
        {

            var response = await _client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();


        }

        public async Task<string> PostAsync(string uri, HttpContent content)
        {

            var response = await _client.PostAsync(uri, content);
            return await response.Content.ReadAsStringAsync();


        }

        public async Task<string> PutAsync(string uri, HttpContent content)
        {
            var response = await _client.PutAsync(uri, content);
            return await response.Content.ReadAsStringAsync();


        }
    }
}
