using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Dtos.Files.Pager;
using FileUpload.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<Response<AddFileDto>> UploadAsync(IFormFile[] files, List<GetCategoryDto> categories);
        Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<GetFileDto>> GetFileById(Guid id);
        Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, Guid fileId);

        Task<Response<string>> Download(string id);


    }
}
