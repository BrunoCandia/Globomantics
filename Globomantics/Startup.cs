using System;
using System.Net.Http;
using Globomantics.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Globomantics
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddSingleton<IConferenceService, ConferenceMemoryService>();
            //services.AddSingleton<IProposalService, ProposalMemoryService>();

            services.AddSingleton<IConferenceService, ConferenceApiService>();
            services.AddSingleton<IProposalService, ProposalApiService>();

            services.AddSingleton(x => new HttpClient { BaseAddress = new Uri("http://localhost:6000") });

            services.Configure<GlobomanticsOptions>(configuration.GetSection("Globomantics"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Conference}/{action=Index}/{id?}");
            });
        }
    }
}
