﻿using AnonFilesUpload.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public class HubTest : Hub
    {
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("receiveMessage", "bağlantı geldi");
        //}

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