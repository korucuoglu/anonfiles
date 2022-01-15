using AnonFilesUpload.Data.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Hubs
{
    public class HubTest : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("receiveMessage", "bağlantı geldi");
        }


        public async Task Upload(AjaxReturningModel model)
        {
            await Clients.All.SendAsync("filesUploaded", model);

        }
    }
}