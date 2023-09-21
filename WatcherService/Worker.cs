namespace WatcherService;

public class Worker : BackgroundService
{
    private readonly FileWatcher _fileWatcher;
    private readonly ILogger<Worker> _logger;

    public Worker(FileWatcher fileWatcher, ILogger<Worker> logger) =>
        (_fileWatcher, _logger) = (fileWatcher, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _fileWatcher.Changed += (sender, e) =>
        {
            _logger.LogInformation("File Change: {name} at {time}", e.Name, DateTimeOffset.Now);
        };
        _fileWatcher.Start();

        try
        {
            // while (!stoppingToken.IsCancellationRequested)
            // {
            //     _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //     await Task.Delay(1000, stoppingToken);
            // }
        }
        catch (TaskCanceledException)
        {
            // Not an error, canceled by external process
            // don't exit
            _fileWatcher.Stop();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            
            // Exit with error code
            Environment.Exit(1);
        }
    }
}
