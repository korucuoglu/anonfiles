﻿using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UploadModel>> Upload(IFormFile file);
        Task<Response<MyFilesViewModel>> GetMyFiles(FileFilterModel model);
        Task<Response<string>> GetDirectLink(string id);
        Task<Response<FileDto>> DeleteFile(FileFilterModel model, string id);







    }
}
