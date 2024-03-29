﻿using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Hub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Upload.Infrastructure.Hub
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
