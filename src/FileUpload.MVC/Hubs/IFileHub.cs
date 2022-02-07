using FileUpload.Shared.Models;
using System.Threading.Tasks;

namespace FileUpload.MVC.Hubs
{
    public interface IFileHub
    {
        Task FilesUploaded(Response<UploadModel> model);
        Task FilesUploadStarting(string fileName);
    }
}
