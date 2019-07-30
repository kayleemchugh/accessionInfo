
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace emailServiceAPITemplate
{
    public class Startup
    {
        HubConnection connection;
        Services.BRCFormService formService;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Console.WriteLine("setting up hub connection");

            connection = new HubConnectionBuilder()
                .WithUrl("https://armypoc.service.signalr.net:5002/api/v1-preview/hub/accessionInfoHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string, Models.BRCInfo>("ReceiveBRCInfo", (user, message) =>
            {
                // do something in other class 
                Models.AccessionBasicInfo basicInfo = formService.extractCareerCodeFromBRCFormInfo(message);
                sendBasicAccesssionInfoToAzure("POST", "URL", basicInfo);
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public void sendBasicAccesssionInfoToAzure(string httpMethod, string URL, Models.AccessionBasicInfo accessionBasicInfo)
        {

            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), URL);

                var httpContent = new StringContent(JsonConvert.SerializeObject(accessionBasicInfo));


                if (accessionBasicInfo != null)


                requestMessage.Content = httpContent;   // This is where your content gets added to the request body


                HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                string apiResponse = response.Content.ReadAsStringAsync().Result;
                try
                {
                    // Attempt to deserialise the reponse to the desired type, otherwise throw an expetion with the response from the api.
                    if (apiResponse != "")
                        Console.WriteLine(JsonConvert.DeserializeObject<Object>(apiResponse));
                    else
                        throw new Exception();
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
        }
    }
}
