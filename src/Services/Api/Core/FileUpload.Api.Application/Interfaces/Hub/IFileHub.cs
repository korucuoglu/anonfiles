using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Wrappers;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Hub
{
    public interface IFileHub
    { 
        Task FilesUploaded(Response<AddFileDto> model);
        Task FilesUploadStarting(string fileName);
    }
}
