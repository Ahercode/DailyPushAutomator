using DailyPushAutomator;
using DailyPushAutomator.Processes;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<Automator>();
    })
    .Build();

host.Run();