using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Owin;

//[assembly: OwinStartup(typeof(accessionInfo.Messaging.Startup))]
namespace accessionInfo.Messaging
{
    public class Startup
    {
        HubConnection connection;

        //public void Configure(IApplicationBuilder app)
        //{

        //    Console.WriteLine("setting up hub connection");

        //    connection = new HubConnectionBuilder()
        //        .WithUrl("https://armypoc.service.signalr.net:5002/api/v1-preview/hub/accessionInfoHub")
        //        .Build();

        //    connection.Closed += async (error) =>
        //    {
        //        await Task.Delay(new Random().Next(0, 5) * 1000);
        //        await connection.StartAsync();
        //    };

        //    connection.On<string, string>("ReceiveMessage", (user, message) =>
        //    {
        //        // do something in other class 
               
        //    });

        //    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //}
    }
}
