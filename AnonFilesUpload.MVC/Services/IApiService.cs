using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services
{
    public interface IApiService
    {
        HttpClient HttpClient { get; set; }
        Task<string> GetAsync(string uri);
        Task<T> GetAllAsync<T>(string uri);


        //	Task<T> AddAsync(T dto);

        //	Task<bool> Remove(int id);
        //	Task<bool> Update(T dto);



    }
}
