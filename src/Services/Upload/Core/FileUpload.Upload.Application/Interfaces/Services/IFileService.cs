using FileUpload.Shared.Wrappers;
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
        Task<Response<AddFileDto>> UploadAsync(IFormFile[] files, List<Guid> CategoriesId);
        Task<Response<FilesPagerViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<GetFileDto>> GetFileById(Guid id);
        Task<Response<FilePagerViewModel>> Remove(FileFilterModel model, Guid fileId);

        Task<Response<NoContent>> Download(string id);


    }
}
