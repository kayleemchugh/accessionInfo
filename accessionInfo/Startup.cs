
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace emailServiceAPITemplate
{
    enum Connection
    {
        server,
        client
    }

    public class Startup
    {
        HubConnection connection;
        Services.BRCFormService formService = new Services.BRCFormService();

        public static readonly string signalRConnectionString = "Endpoint=https://armypoc.service.signalr.net;AccessKey=ewyrRfLNd7Rd5aEdEHm6Xx2dmFcg08SrKJ52J2nEAoQ=;Version=1.0;";
        public static readonly string accessionInfoHub = "AccessionInfoHub";
        public static readonly string accessionInfoHubConnectionUrl = "https://armypoc.service.signalr.net/client/?hub=AccessionInfoHub";
        public static readonly string orchestrationAzureFunction = "https://brc-form-to-email-orch.azurewebsites.net/api/BRC_form_to_email_orch";
        public static readonly string targetMessage = "BrcFormSubmit";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Console.WriteLine("Setting up hub connection");

             ServiceUtils serviceUtils = new ServiceUtils(nameof(Connection.client), signalRConnectionString, accessionInfoHub);
             string token = serviceUtils.GenerateAccessToken();

             connection = new HubConnectionBuilder().WithUrl(accessionInfoHubConnectionUrl, option =>
                {
                    option.AccessTokenProvider = () =>
                    {
                        return Task.FromResult(token);
                    };
                }).Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<Models.BRCInfo>(targetMessage, (message) =>
            {
                // do something in other class 
                Models.AccessionBasicInfo basicInfo = formService.extractCareerCodeFromBRCFormInfo(message);
                string code = basicInfo.careerCode;
                
                sendBasicAccesssionInfoToAzure("POST", orchestrationAzureFunction, basicInfo);
            });

             StartAsync();
        }

        public async Task StartAsync()
        {
            await connection.StartAsync();
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

                var httpContent = new StringContent(JsonConvert.SerializeObject(accessionBasicInfo), System.Text.Encoding.UTF8, "application/json");


                if (accessionBasicInfo != null)


                requestMessage.Content = httpContent;   // This is where your content gets added to the request body


                HttpResponseMessage response = client.SendAsync(requestMessage).Result;
                HttpStatusCode apiResponse = response.StatusCode;
                
                try
                {
                    // Attempt to deserialise the reponse to the desired type, otherwise throw an expetion with the response from the api.
                    if (apiResponse != HttpStatusCode.Accepted )
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
