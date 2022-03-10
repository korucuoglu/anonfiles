using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Files;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Hub
{
    public interface IFileHub
    { 
        Task FilesUploaded(Response<AddFileDto> model);
        Task FilesUploadStarting(string fileName);
    }
}
