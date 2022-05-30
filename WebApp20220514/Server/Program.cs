using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace WebApp20220514.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            log4net.Config.XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()), log4netConfig["log4net"]);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    // clear default logging providers
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventLog();
                    // add more providers here
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                //.ConfigureLogging(logging =>
                //{
                //    logging.ClearProviders();
                //    logging.SetMinimumLevel(LogLevel.Trace);
                //})
                //.UseNLog();
        //.UseSerilog();
        //.ConfigureLogging(builder =>
        //{
        //    builder.AddLog4Net("log4net.config");
        //});
    }
}
