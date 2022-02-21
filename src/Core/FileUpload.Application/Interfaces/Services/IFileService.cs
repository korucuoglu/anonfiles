using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Features.Commands.Files.Add;
using FileUpload.Application.Wrappers;
using System;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<Response<UploadModel>> UploadAsync(AddFileCommand dto);
        Task<Response<MyFilesViewModel>> GetAllFiles(FileFilterModel model);
        Task<Response<FileDto>> GetFileById(Guid id);


    }
}
