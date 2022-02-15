using FileUpload.Shared.Models;
using System.Threading.Tasks;

namespace FileUpload.Api.Hubs
{
    public interface IFileHub
    {
        Task FilesUploaded(Response<UploadModel> model);
        Task FilesUploadStarting(string fileName);
    }
}
