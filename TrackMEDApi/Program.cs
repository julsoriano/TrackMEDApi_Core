using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace TrackMEDApi
{
    public class Program
    {
        public static int Main(string[] args)
        // public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseSerilog();

            /* If you use CreateDefaultBuilder, you can replace the default providers with your own choices. 
                 Call ClearProviders, and add the providers you want. 
                 - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1&tabs=aspnetcore2x#built-in-logging-providers

            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });
            */
        }
    }
