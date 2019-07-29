using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace emailServiceAPITemplate.Messaging
{

    namespace emailServiceAPITemplate.Messaging
    {
        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();
                services.AddSignalR()
                        .AddAzureSignalR();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMvc();
                app.UseFileServer();
                app.UseAzureSignalR(routes =>
                {
                    routes.MapHub<SignalR>("/accessionBasicInfo");
                });
            }
        }


    }


}
