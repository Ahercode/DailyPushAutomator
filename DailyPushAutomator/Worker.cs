using DailyPushAutomator.Processes;

namespace DailyPushAutomator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Automator _automator;
    public Worker(ILogger<Worker> logger, Automator automator)
    {
        _logger = logger;
        _automator = automator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _automator.PushToRepoAsync();
            await Task.Delay(1000, stoppingToken);
        }
    }
}