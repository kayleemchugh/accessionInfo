using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

//written from this documentation
//https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio-mac

namespace emailServiceAPITemplate.Messaging
{
    public class SignalR : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
