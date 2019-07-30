using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR.Client;


//written from this documentation
//https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio-mac

namespace emailServiceAPITemplate.Messaging
{
    [HubName("AccessionInfoHub")]
    public class AccessionInfoHub : Hub
    {
        public HubConnection hubConnection;

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //private async Task get_BRC_info(string user, string message)
        //{

 

        //}
    }
}
