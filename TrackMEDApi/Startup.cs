using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using TrackMEDApi.Extensions;

namespace TrackMEDApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
            /*
            EmailConnectionInfo info = new EmailConnectionInfo()
            {
                EmailSubject = "Test Serilog: Using smtp.live.com",
                FromEmail = "jul_soriano@hotmail.com",
                MailServer = "smtp.live.com",
                EnableSsl = true,
                NetworkCredentials = new NetworkCredential("jul_soriano@hotmail.com", "acts15:23hot"),
                Port = 587,
                ToEmail = "jul_soriano@yahoo.com"
            };
            */
            
            EmailConnectionInfo info = new EmailConnectionInfo()
            {
                EmailSubject = "Test Serilog: Using smtp.gmail.com",
                FromEmail = "elijahpne@gmail.com",
                MailServer = "smtp.gmail.com",
                // EnableSsl = true,
                NetworkCredentials = new NetworkCredential("elijahpne@gmail.com", "juDe3GOOG"),
                Port = 465, // 587
                ToEmail = "jul_soriano@yahoo.com"
            };
            /*  Port = 465, MailServer = "smtp.gmail.com", EnableSsl = false
             *  
                [12:19:24 WRN] Starting web host: TrackMEDApi_Core
                [12:19:24 INF] User profile is available. Using 'C:\Users\Julito\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
                Hosting environment: Development
                Content root path: C:\CIT\VS\Workspaces\RESMED\TrackMEDApi_Core\TrackMEDApi
                Now listening on: https://localhost:5001
                Now listening on: http://localhost:5000
                Application started. Press Ctrl+C to shut down.
                [12:19:27 INF] Request starting HTTP/1.1 GET http://localhost:5000/api/status
                [12:19:27 INF] Request finished in 96.9392ms 307
                [12:19:27 INF] Request starting HTTP/1.1 GET https://localhost:5001/api/status
                [12:19:27 INF] Executing endpoint 'TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)'
                [12:19:27 INF] Route matched with {action = "GetAllAsync", controller = "Status"}. Executing action TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)
                [12:19:28 INF] Executing action method TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi) - Validation state: Valid
                [12:19:29 INF] Executed action method TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi), returned result Microsoft.AspNetCore.Mvc.ObjectResult in 814.7817ms.
                [12:19:29 INF] Executing ObjectResult, writing value of type 'System.Collections.Generic.List`1[[TrackMEDApi.Models.Status, TrackMEDApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]'.
                [12:19:29 INF] Executed action TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi) in 2115.8524ms
                [12:19:29 INF] Executed endpoint 'TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)'
                [12:19:29 INF] Request finished in 2452.3145ms 200 application/json; charset=utf-8
                2019-05-14T04:21:25.0526410Z Failed to send email: System.IO.IOException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond ---> System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond
                   at MailKit.Net.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 count)
                   --- End of inner exception stack trace ---
                   at MailKit.Net.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 count)
                   at MailKit.Net.Smtp.SmtpStream.ReadAheadAsync(Boolean doAsync, CancellationToken cancellationToken)
                   at MailKit.Net.Smtp.SmtpStream.ReadResponseAsync(Boolean doAsync, CancellationToken cancellationToken)
                   at MailKit.Net.Smtp.SmtpStream.ReadResponse(CancellationToken cancellationToken)
                   at MailKit.Net.Smtp.SmtpClient.ConnectAsync(String host, Int32 port, SecureSocketOptions options, Boolean doAsync, CancellationToken cancellationToken)
                   at MailKit.Net.Smtp.SmtpClient.Connect(String host, Int32 port, SecureSocketOptions options, CancellationToken cancellationToken)
                   at MailKit.MailService.Connect(String host, Int32 port, Boolean useSsl, CancellationToken cancellationToken)
                   at Serilog.Sinks.Email.EmailSink.OpenConnectedSmtpClient()
                   at Serilog.Sinks.Email.EmailSink.EmitBatchAsync(IEnumerable`1 events
            */
            /* Port = 465 or 587, MailServer = "smtp.gmail.com", EnableSsl = true
             * 
            [12:32:35 WRN] Starting web host: TrackMEDApi_Core
            [12:32:35 INF] User profile is available. Using 'C:\Users\Julito\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
            2019-05-14T04:32:36.3017572Z Failed to send email: MailKit.Security.SslHandshakeException: An error occurred while attempting to establish an SSL or TLS connection.

            One possibility is that you are trying to connect to a port which does not support SSL/TLS.

            The other possibility is that the SSL certificate presented by the server is not trusted by the system for one or more of the following reasons:
            1. The server is using a self-signed certificate which cannot be verified.
            2. The local system is missing a Root or Intermediate certificate needed to verify the server's certificate.
            3. The certificate presented by the server is expired or invalid.

            See https://github.com/jstedfast/MailKit/blob/master/FAQ.md#InvalidSslCertificate for possible solutions. ---> System.Security.Authentication.AuthenticationException: The remote certificate is invalid according to the validation procedure.
               at System.Net.Security.SslState.StartSendAuthResetSignal(ProtocolToken message, AsyncProtocolRequest asyncRequest, ExceptionDispatchInfo exception)
               at System.Net.Security.SslState.CheckCompletionBeforeNextReceive(ProtocolToken message, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartSendBlob(Byte[] incoming, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.ProcessReceivedBlob(Byte[] buffer, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartReadFrame(Byte[] buffer, Int32 readBytes, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartReceiveBlob(Byte[] buffer, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.CheckCompletionBeforeNextReceive(ProtocolToken message, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartSendBlob(Byte[] incoming, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.ProcessReceivedBlob(Byte[] buffer, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartReadFrame(Byte[] buffer, Int32 readBytes, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartReceiveBlob(Byte[] buffer, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.CheckCompletionBeforeNextReceive(ProtocolToken message, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartSendBlob(Byte[] incoming, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.ProcessReceivedBlob(Byte[] buffer, Int32 count, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.StartReadFrame(Byte[] buffer, Int32 readBytes, AsyncProtocolRequest asyncRequest)
               at System.Net.Security.SslState.PartialFrameCallback(AsyncProtocolRequest asyncRequest)
            --- End of stack trace from previous location where exception was thrown ---
               at System.Net.Security.SslState.ThrowIfExceptional()
               at System.Net.Security.SslState.InternalEndProcessAuthentication(LazyAsyncResult lazyResult)
               at System.Net.Security.SslState.EndProcessAuthentication(IAsyncResult result)   at System.Net.Security.SslStream.EndAuthenticateAsClient(IAsyncResult asyncResult)
               at System.Net.Security.SslStream.<>c.<AuthenticateAsClientAsync>b__46_2(IAsyncResult iar)
               at System.Threading.Tasks.TaskFactory`1.FromAsyncCoreLogic(IAsyncResult iar, Func`2 endFunction, Action`1 endAction, Task`1 promise, Boolean requiresSynchronization)
            --- End of stack trace from previous location where exception was thrown ---
               at MailKit.Net.Smtp.SmtpClient.ConnectAsync(String host, Int32 port, SecureSocketOptions options, Boolean doAsync, CancellationToken cancellationToken)
               --- End of inner exception stack trace ---
               at MailKit.Net.Smtp.SmtpClient.ConnectAsync(String host, Int32 port, SecureSocketOptions options, Boolean doAsync, CancellationToken cancellationToken)
               at MailKit.Net.Smtp.SmtpClient.Connect(String host, Int32 port, SecureSocketOptions options, CancellationToken cancellationToken)
               at MailKit.MailService.Connect(String host, Int32 port, Boolean useSsl, CancellationToken cancellationToken)
               at Serilog.Sinks.Email.EmailSink.OpenConnectedSmtpClient()
               at Serilog.Sinks.Email.EmailSink.EmitBatchAsync(IEnumerable`1 events)
            Hosting environment: Development
            Content root path: C:\CIT\VS\Workspaces\RESMED\TrackMEDApi_Core\TrackMEDApi
            Now listening on: https://localhost:5001
            Now listening on: http://localhost:5000
            Application started. Press Ctrl+C to shut down.
            [12:32:38 INF] Request starting HTTP/1.1 GET http://localhost:5000/api/status
            [12:32:38 INF] Request finished in 178.6448ms 307
            [12:32:39 INF] Request starting HTTP/1.1 GET https://localhost:5001/api/status
            [12:32:39 INF] Executing endpoint 'TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)'
            [12:32:39 INF] Route matched with {action = "GetAllAsync", controller = "Status"}. Executing action TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)
            [12:32:40 INF] Executing action method TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi) - Validation state: Valid
            [12:32:41 INF] Executed action method TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi), returned result Microsoft.AspNetCore.Mvc.ObjectResult in 1233.8136ms.
            [12:32:41 INF] Executing ObjectResult, writing value of type 'System.Collections.Generic.List`1[[TrackMEDApi.Models.Status, TrackMEDApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]'.
            [12:32:42 INF] Executed action TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi) in 2902.9024ms
            [12:32:42 INF] Executed endpoint 'TrackMEDApi.Controllers.StatusController.GetAllAsync (TrackMEDApi)'
            [12:32:42 INF] Request finished in 3262.0194ms 200 application/json; charset=utf-8
            
             */

            // Configure Serilog 
            // Version 1: Using AppSettings (Serilog)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console()
                /*
                .WriteTo.Email(info,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
                    restrictedToMinimumLevel: LogEventLevel.Warning)   // https://github.com/serilog/serilog/wiki/Configuration-Basics#overriding-per-sink
                
                .WriteTo.Email(new EmailConnectionInfo
                {
                    FromEmail = "jul_soriano@hotmail.com",
                    ToEmail = "jul_soriano@yahoo.com",
                    MailServer = "smtp.live.com",
                    NetworkCredentials = new NetworkCredential
                    {
                        UserName = "jul_soriano@hotmail.com",
                        Password = "acts15:23hot"
                    },
                    EnableSsl = true,
                    Port = 587,
                    EmailSubject = "Test Serilog"
                },              
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
                // batchPostingLimit: 10
                , restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning
                )
                */
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
                     
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            
                options.AddPolicy("AllowAllMethods",
                    builder =>
                    {
                        //builder.WithOrigins("http://example.com")
                        //        .AllowAnyMethod();
                        builder.AllowAnyMethod();
                    });
            
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyHeader();
                    });
                // END06

                // BEGIN07
                options.AddPolicy("ExposeResponseHeaders",
                    builder =>
                    {
                        builder.WithExposedHeaders("x-custom-header");
                    });
                // END07

                // BEGIN08
                options.AddPolicy("AllowCredentials",
                    builder =>
                    {
                        builder.DisallowCredentials();
                    });
                // END08

                // BEGIN09
                options.AddPolicy("SetPreflightExpiration",
                    builder =>
                    {
                        builder.SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                    });
                // END09

                // BEGIN10
                options.AddPolicy("AllowSubdomain",
                    builder =>
                    {
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
                // END11
            
            });
            

            services.AddRazorPages();

            // Add our Config object so it can be injected
            services.Configure<Settings>(Configuration.GetSection("MongoSettings"));

            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /* Migrating from .Net Core 2.2 to 3.0, See https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio
                 or See: https://www.evernote.com/shard/s102/nl/11219721/b69c73f9-4dbe-4688-baef-1513c808e046                         
            */
            app.UseRouting();   // Replaces useMVC

            // For most apps, calls to UseAuthentication, UseAuthorization, and UseCors must appear between the calls to UseRouting and UseEndpoints to be effective.
            // See https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-2.2&tabs=visual-studio#break
            // app.UseCors("default");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Status}/{action=Index}/{id?}");
                    /* Note: 
                     * There is no action in StatusController named 'Index'. 
                     * Routing probably looks at the signature rather than the action name
                          so this defaults to GetAllAsync which has the specified signature.
                     * Works in conjunction with launchsettings.json. THIS JSON MUST BE SPECIFIED
                    */
            });
            
        }
    }
}
