using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaystationWishlist.AutoMapper;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(PlaystationWishlistFunctionsOrchestrator.Startup))]
namespace PlaystationWishlistFunctionsOrchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() //this doesnt do anything useful notice im setting some env variables explicitly. 
                .Build();  //build it so you can use those config variables down below.

            var services = builder.Services;

            services.AddAutoMapper(typeof(PlaystationGameProfile).Assembly);
            services.AddTransient(typeof(IPlaystationWishlistDbContext), typeof(PlaystationWishlistContext));
            services.AddDbContext<PlaystationWishlistContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new ArgumentNullException("Connection string not configurated."));
            });
            services.AddDbContext<IdentityAppContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new ArgumentNullException("Connection string not configurated."));
            });
        }
    }
}