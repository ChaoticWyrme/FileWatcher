using WatcherService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "File Watcher Service";
});
builder.Configuration.AddCommandLine(args);
    
LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddSingleton(new FileWatcher(args[0]));
builder.Services.AddHostedService<Worker>();
Console.WriteLine($"{builder.Configuration.Sources}");
var host = builder.Build();
host.Run();
