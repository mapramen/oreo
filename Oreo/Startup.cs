using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Ably;
using IO.Ably.Realtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oreo.Bots;
using Oreo.Games;

namespace Oreo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            var realtime = new AblyRealtime(new ClientOptions(this.Configuration.GetValue<string>("AblyApiKey")) { 
                EchoMessages = false
            });
            
            realtime.Connection.On(ConnectionEvent.Connected, args =>
            {
                Console.Out.WriteLine("That was simple, you're now connected to Ably in realtime");
            });
            services.AddSingleton<IChannels<IRealtimeChannel>>(realtime.Channels);

            services.AddSingleton<IGameRunnerFactory, GameRunnerFactory>();
            services.AddSingleton<IGameRunnerRepository, GameRunnerRepository>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, TeamsMessagingExtensionsBot>();

            services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));

            services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
