# BlazorWASMApp20220514

### [Download Bootstrap 5](https://getbootstrap.com/docs/5.0/getting-started/download/)


#### [Serilog](https://serilog.net/)

https://github.com/serilog/serilog/wiki/Getting-Started

Install NuGet Package
```
Serilog
Serilog.Sinks.Console
Serilog.Sinks.File
```

```
 Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
               .CreateLogger();
```

example
```
Log.Information("Hello, Serilog!");
Log.Warning("Hello, Serilog!");
Log.Error("Hello, Serilog!");
Log.Debug("Hello, Serilog!");
```

[NLog](https://nlog-project.org/config/)
```
NLog.Web.AspNetCore
NLog
```
Create nlog.config (lowercase all) file in the root of your project.

We use this example:
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="c:\temp\internal-nlog-AspNetCore.txt">

  <!-- enable asp.net core layout renderers -->
  <extensions>
    <add assembly="NLog.Web.AspNetCore"/>
  </extensions>

  <!-- the targets to write to -->
  <targets>
    <!-- File Target for all log messages with basic details -->
    <target xsi:type="File" name="allfile" fileName="c:\temp\nlog-AspNetCore-all-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}" />

    <!-- File Target for own log messages with extra web details using some ASP.NET core renderers -->
    <target xsi:type="File" name="ownFile-web" fileName="c:\temp\nlog-AspNetCore-own-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}|${callsite}" />

    <!--Console Target for hosting lifetime messages to improve Docker / Visual Studio startup detection -->
    <target xsi:type="Console" name="lifetimeConsole" layout="${MicrosoftConsoleLayout}" />
  </targets>

  <!-- rules to map from logger name to target -->
  <rules>
    <!--All logs, including from Microsoft-->
    <logger name="*" minlevel="Trace" writeTo="allfile" />

    <!--Output hosting lifetime messages to console target for faster startup detection -->
    <logger name="Microsoft.Hosting.Lifetime" minlevel="Info" writeTo="lifetimeConsole, ownFile-web" final="true" />

    <!--Skip non-critical Microsoft logs and so log only own logs (BlackHole) -->
    <logger name="Microsoft.*" maxlevel="Info" final="true" />
    <logger name="System.Net.Http.*" maxlevel="Info" final="true" />
    
    <logger name="*" minlevel="Trace" writeTo="ownFile-web" />
  </rules>
</nlog>
```

add in startup.cs
```
 public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
```
NLog Example Code
```
using Microsoft.Extensions.Logging;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into HomeController");
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Hello, this is the index!");
        return View();
    }
}
```
[You can add custom config](https://nlog-project.org/config/)


https://github.com/thecodebuzz/nlog-database-logging-config-file-example/blob/master/DDL.txt

https://thesoftwayfarecoder.com/integrating-log4net-and-using-adonetappender-in-asp-net-core/

https://github.com/NLog/NLog/wiki/Database-target#dbprovider-examples

| Log Name | Text Log | Db Log  |
|----------|----------|---------|
| Serilog  | Success  | Success |
| NLog     | Success  | Fail    |
| Log4net  | Success  | Fail    |