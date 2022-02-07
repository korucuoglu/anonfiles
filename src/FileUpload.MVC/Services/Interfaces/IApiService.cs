using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services.Interfaces
{
    public interface IApiService
    {
        Task<string> GetAsync(string uri);
        Task<string> DeleteAsync(string uri);
        Task<string> PutAsync(string uri, HttpContent content);
        Task<string> PostAsync(string uri, HttpContent content);
    }


}
