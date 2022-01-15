﻿using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
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

        Task<Response<AjaxReturningModel>> Upload(IFormFile file, string uri);
        Task<string> UploadTest(IFormFile file);

        //	Task<bool> Remove(int id);
        //	Task<bool> Update(T dto);



    }
}
