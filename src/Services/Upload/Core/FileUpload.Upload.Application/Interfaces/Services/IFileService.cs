﻿using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<Response<AddFileDto>> UploadAsync(IFormFile file, List<int> CategoriesId);
        Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<GetFileDto>> GetFileById(int id);
        Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, int id);

        Task<Response<NoContent>> Download(int id);

        Task<Response<string>> GetFileKeyById(int id);


    }
}