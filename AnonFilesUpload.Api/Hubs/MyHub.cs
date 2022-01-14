using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Hubs
{
    public class MyHub: Hub
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
        }
    }
}
