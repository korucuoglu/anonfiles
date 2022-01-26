using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public class FileHub : Hub
    {
    
        public async Task Upload(Response<UploadModel> model)
        {
            await Clients.All.SendAsync("filesUploaded", model.Data);
        }

        public async Task UploadStarting(string fileName)
        {
            await Clients.All.SendAsync("filesUploadedStarting", fileName);
        }
    }
}