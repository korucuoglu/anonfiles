using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Extension
{
    public static class HttpClientExtension
    {
        public async static Task<TResult> CustomGetAsync<TResult>(this HttpClient client, string url)
        {
            return await client.GetFromJsonAsync<TResult>(url);
        }

        public async static Task<TResult> CustomPostAsync<TResult>(this HttpClient client, string url, HttpContent content)
        {
            var result = await client.PostAsync(url, content);

            return await result.Content.ReadFromJsonAsync<TResult>();
        }

        public async static Task<TResult> CustomPostAsync<TResult, TValue>(this HttpClient Client, String Url, TValue Value)
        {
            var result = await Client.PostAsJsonAsync(Url, Value);

            return await result.Content.ReadFromJsonAsync<TResult>();
        }

        public async static Task<TResult> CustomDeleteAsync<TResult>(this HttpClient client, string url) where TResult : class
        {
            var result = await client.DeleteAsync(url);

            return await result.Content.ReadFromJsonAsync<TResult>();
        }

        public async static Task<TResult> CustomPutAsync<TResult, TValue>(this HttpClient Client, String Url, TValue Value)
        {
            var httpRes = await Client.PutAsJsonAsync(Url, Value);

            return await httpRes.Content.ReadFromJsonAsync<TResult>();
        }
    }
}
