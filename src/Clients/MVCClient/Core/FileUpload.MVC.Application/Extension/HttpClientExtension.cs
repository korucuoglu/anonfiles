using FileUpload.MVC.Application.Exceptions;
using FileUpload.Shared.Wrappers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Extension
{
    public static class HttpClientExtension
    {
        public async static Task<TResult> CustomPostAsyncWithHttpContent<TResult>(this HttpClient Client, String Url, HttpContent Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PostAsync(Url, Value);

            if (!httpRes.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }
            var res = await httpRes.Content.ReadFromJsonAsync<Response<TResult>>();

            return !res.IsSuccessful && ThrowSuccessException ? throw new ApiException(res.Error) : res.Value;
        }

        public async static Task<TResult> CustomPostAsync<TResult, TValue>(this HttpClient Client, String Url, TValue Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PostAsJsonAsync(Url, Value);

            if (!httpRes.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }
            var res = await httpRes.Content.ReadFromJsonAsync<Response<TResult>>();

            return !res.IsSuccessful && ThrowSuccessException ? throw new ApiException(res.Error) : res.Value;
        }

        public async static Task<Response<TResult>> CustomPostAsyncReturnsResponse<TResult, TValue>(this HttpClient Client, String Url, TValue Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PostAsJsonAsync(Url, Value);

            if (!httpRes.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }
            var res = await httpRes.Content.ReadFromJsonAsync<Response<TResult>>();

            return !res.IsSuccessful && ThrowSuccessException ? throw new ApiException(res.Error) : res;
        }

        public async static Task<T> CustomGetAsync<T>(this HttpClient Client, String Url, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.GetFromJsonAsync<Response<T>>(Url);

            return !httpRes.IsSuccessful && ThrowSuccessException ? throw new ApiException(httpRes.Error) : httpRes.Value;
        }

        public async static Task<Response<T>> CustomGetAsyncReturnsResponse<T>(this HttpClient Client, String Url, bool ThrowSuccessException = false)
        {
            var request = await Client.GetAsync(Url);

            if (!request.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }

            var httpRes = JsonConvert.DeserializeObject<Response<T>>(await request.Content.ReadAsStringAsync());

            return !httpRes.IsSuccessful && ThrowSuccessException ? throw new ApiException(httpRes.Error) : httpRes;
        }

        public async static Task<T> CustomDeleteAsync<T>(this HttpClient Client, String Url, bool ThrowSuccessException = false)
        {
            var request = await Client.DeleteAsync(Url);

            if (!request.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }

            var httpRes = JsonConvert.DeserializeObject<Response<T>>(await request.Content.ReadAsStringAsync());

            return !httpRes.IsSuccessful && ThrowSuccessException ? throw new ApiException(httpRes.Error) : httpRes.Value;
        }

        public async static Task<Response<T>> CustomDeleteAsyncReturnsResponse<T> (this HttpClient Client, String Url, bool ThrowSuccessException = false)
        {
            var request = await Client.DeleteAsync(Url);

            if (!request.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }

            var httpRes = JsonConvert.DeserializeObject<Response<T>>(await request.Content.ReadAsStringAsync());

            return !httpRes.IsSuccessful && ThrowSuccessException ? throw new ApiException(httpRes.Error) : httpRes;
        }

        public async static Task<TResult> CustomPutAsync<TResult, TValue>(this HttpClient Client, String Url, TValue Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PutAsJsonAsync(Url, Value);

            if (!httpRes.IsSuccessStatusCode)
            {
                throw new ApiException("hata meydana geldi");
            }
            var res = await httpRes.Content.ReadFromJsonAsync<Response<TResult>>();

            return !res.IsSuccessful && ThrowSuccessException ? throw new ApiException(res.Error) : res.Value;

        }
    }
}
