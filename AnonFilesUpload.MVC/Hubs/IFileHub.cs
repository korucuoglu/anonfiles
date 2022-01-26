using AnonFilesUpload.Shared.Models;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public interface IFileHub
    {
        Task FilesUploaded(Response<UploadModel> model);
        Task FilesUploadStarting(string fileName);
    }
}
