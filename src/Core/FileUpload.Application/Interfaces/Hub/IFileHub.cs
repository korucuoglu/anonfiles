using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Wrappers;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Hub
{
    public interface IFileHub
    { 
        Task FilesUploaded(Response<UploadModel> model);
        Task FilesUploadStarting(string fileName);
    }
}
