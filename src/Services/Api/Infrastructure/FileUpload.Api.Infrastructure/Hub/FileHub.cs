using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Hub;
using FileUpload.Application.Wrappers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure.Hub
{
    public class FileHub : Hub<IFileHub>
    {
        public override Task OnConnectedAsync()
        {
            var UserId = "1";

            if (!HubData.ClientsData.Any(x => x.UserId == UserId))
            {
                var user = new HubDataModel()
                {
                    UserId = UserId,
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


        public async Task FilesUploaded(Response<AddFileDto> model)
        {
            await Clients.Caller.FilesUploaded(model);
        }

        public async Task FilesUploadStarting(string fileName)
        {
            await Clients.Caller.FilesUploadStarting(fileName);
        }
    }
}
