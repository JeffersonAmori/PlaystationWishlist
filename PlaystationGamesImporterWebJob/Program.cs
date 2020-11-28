using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlaystationWishlist.AutoMapper;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Data;
using PlaystationWishlist.KeyVault;

namespace PlaystationGamesImporterWebJob
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();
            // Configure DI
            ServiceLocator.Instance = ConfigureServices().BuildServiceProvider();

            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
                b.AddTimers();
            });

            builder.ConfigureServices((context, s) =>
            {
                s = ConfigureServices();
                s.BuildServiceProvider();
            });

            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
                // If the key exists in settings, use it to enable Application Insights.
                string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
                }
            });

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() //this doesnt do anything useful notice im setting some env variables explicitly. 
                .Build();  //build it so you can use those config variables down below.

            var services = new ServiceCollection();
            services.AddTransient(typeof(IPlaystationWishlistDbContext), typeof(PlaystationWishlistContext));
            services.AddTransient(typeof(IKeyVaultService), typeof(KeyVaultService));
            services.AddAutoMapper(typeof(PlaystationGameProfile).Assembly);

            return services;
        }
    }
}
