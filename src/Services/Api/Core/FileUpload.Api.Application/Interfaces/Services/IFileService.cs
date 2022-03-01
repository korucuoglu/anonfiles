using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Dtos.Files.Pager;
using FileUpload.Api.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<Response<AddFileDto>> UploadAsync(IFormFile[] files, List<string> CategoriesId);
        Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<GetFileDto>> GetFileById(string id);
        Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, string fileId);

        Task<Response<string>> Download(string id);


    }
}
