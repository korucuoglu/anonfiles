
using FileUpload.Api.Models;
using FileUpload.Shared.Models;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace FileUpload.Api.Hubs
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

            if (!HubData.ClientsData.Any(x => x.UserId == "1"))
            {
                var user = new HubDataModel()
                {
                    UserId = "1",
                    ConnectionId = Context.ConnectionId
                };

                HubData.ClientsData.Add(user);
            }


            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            var user = HubData.ClientsData.FirstOrDefault(x => x.UserId == "1");
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