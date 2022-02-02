using AnonFilesUpload.MVC.Models;
using AnonFilesUpload.Shared.Services;
using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public class FileHub : Hub<IFileHub>
    {
        
        private readonly ISharedIdentityService _sharedIdentityService;
        public FileHub(ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
        }

        public override Task OnConnectedAsync()
        {
            if(!HubData.ClientsData.Any(x=> x.UserId == _sharedIdentityService.GetUserId))
            {
                var user = new HubDataModel()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = _sharedIdentityService.GetUserId
                };

                HubData.ClientsData.Add(user);
            }


            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            var user = HubData.ClientsData.FirstOrDefault(x => x.UserId == _sharedIdentityService.GetUserId);
            HubData.ClientsData.Remove(user);
            return Task.CompletedTask;
        }


        public async Task FilesUploaded(Response<UploadModel> model)
        {
            await Clients.Caller.FilesUploaded(model);
        }

        public async Task FilesUploadStarting(string fileName)
        {
            await Clients.Caller.FilesUploadStarting(fileName);
        }
    }
}