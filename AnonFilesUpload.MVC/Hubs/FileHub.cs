using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public class FileHub : Hub<IFileHub>
    {

        public async Task Upload(Response<UploadModel> model)
        {
            await Clients.Caller.FilesUploaded(model);
        }

        public async Task UploadStarting(string fileName)
        {
            await Clients.Caller.FilesUploadStarting(fileName);
        }
    }
}