using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceLocator.Instance = services.BuildServiceProvider();

            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
                b.AddTimers();
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

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient(typeof(IPlaystationWishlistDbContext), typeof(PlaystationWishlistContext));
            services.AddTransient(typeof(IKeyVaultService), typeof(KeyVaultService));
        }
    }
}
