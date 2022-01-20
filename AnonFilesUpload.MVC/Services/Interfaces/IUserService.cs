using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> GetGenericAsync(string uri);

        Task<Response<AjaxReturningModel>> Upload(IFormFile file, string uri);
      

       



    }
}
