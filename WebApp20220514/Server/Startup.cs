using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace WebApp20220514.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//We set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .CreateLogger();

            //string filePath = "logs/WebApp20220514.Server.txt";
            string filePath = Path.Combine(configuration.GetSection("LogFolderPath").Value, "WebApp20220514.Server.txt");
            Log.Logger = new LoggerConfiguration()
               //.MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
               .CreateLogger();

            Log.Information("Hello, Serilog!");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Log.Information("Start, UseHttpsRedirection!");
            app.UseHttpsRedirection();
            Log.Information("Finish, UseHttpsRedirection!");

            Log.Information("Start, UseHttpsRedirection!");
            app.UseBlazorFrameworkFiles();
            Log.Information("Finish, UseBlazorFrameworkFiles!");

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
