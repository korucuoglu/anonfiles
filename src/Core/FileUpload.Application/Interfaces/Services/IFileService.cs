using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<Response<UploadModel>> UploadAsync(IFormFile[] files);
        Task<Response<MyFilesViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<FileDto>> GetFileById(Guid id);
        Task<Response<MyFileViewModel>> Remove(FileFilterModel model, Guid fileId);

        Task<Response<string>> Download(string id);


    }
}
