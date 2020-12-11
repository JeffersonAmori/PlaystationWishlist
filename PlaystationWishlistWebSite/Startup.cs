using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaystationWishlist.AutoMapper;
using PlaystationWishlist.DataAccess.Data;
using PlaystationWishlist.DataAccess.Models.Identity;
using System;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite
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
            services.AddControllersWithViews();

            services.AddIdentity<AppUser, AppRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdentityAppContext>();

            services.AddDbContext<IdentityAppContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("ConnectionString") ??
                                 throw new ArgumentNullException("Connection string not configurated."));
            });

            services.AddAuthentication()
                .AddCookie()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.ClientId = Environment.GetEnvironmentVariable("FACEBOOK_CLIENT_ID");
                    facebookOptions.ClientSecret = Environment.GetEnvironmentVariable("FACEBOOK_CLIENT_SECRET");
                })
                .AddMicrosoftAccount(microsoftOption =>
                {
                    microsoftOption.ClientId = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID");
                    microsoftOption.ClientSecret = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_SECRET");
                });

            services.AddHttpClient();
            services.AddAutoMapper(typeof(PlaystationGameProfile).Assembly);
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
