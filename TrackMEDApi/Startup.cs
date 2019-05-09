using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using TrackMEDApi.Extensions;

namespace TrackMEDApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Configure Serilog 
            // Version 1: Using AppSettings (Serilog)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console()
                .WriteTo.Email(
                    fromEmail: "elijahpne@gmail.com",
                    toEmail: "jul_soriano@yahoo.com",
                    mailServer: "smtp.gmail.com")
                .CreateLogger();

            // Version 2: Ignoring AppSettings
            /* var uriMongoDB = Configuration.GetValue<string>("MongoSettings:mongoconnection");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MongoDBCapped(uriMongoDB, collectionName: "logsTrackMEDApi")  // https://github.com/serilog/serilog-sinks-mongodb
                .CreateLogger();
            */

            Log.Warning("Starting web host: TrackMEDApi_Core");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            // Also: Add service and create Policy with options, See https://www.codeproject.com/Articles/1151842/Using-MongoDB-NET-Driver-with-NET-Core-WebAPI
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add our Config object so it can be injected
            services.Configure<Settings>(Configuration.GetSection("MongoSettings"));

            services.RegisterServices();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
