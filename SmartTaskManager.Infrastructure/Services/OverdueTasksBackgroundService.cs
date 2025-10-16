using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartTaskManager.Core.Services;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.Infrastructure.Services;

public class OverdueTasksBackgroundService : BackgroundService
{
    private readonly ILogger<OverdueTasksBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly BackgroundServiceSettings _settings;

    public OverdueTasksBackgroundService(
        ILogger<OverdueTasksBackgroundService> logger,
        IServiceProvider serviceProvider,
        IOptions<BackgroundServiceSettings> settings)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Overdue Tasks Background Service is starting. Check interval: {Interval} minutes",
            _settings.CheckIntervalMinutes);

        await Task.Delay(TimeSpan.FromSeconds(_settings.InitialDelaySeconds), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

                await CheckAndMarkOverdueTasks(taskService, stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(_settings.CheckIntervalMinutes), stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error occurred executing background work");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        _logger.LogInformation("Overdue Tasks Background Service is stopping");
    }

    private async Task CheckAndMarkOverdueTasks(ITaskService taskService, CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting overdue tasks check at {Time}", DateTime.UtcNow);

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var now = DateTime.UtcNow;
        var overdueTasks = await dbContext.Tasks
            .Where(t => !t.IsCompleted && !t.IsOverdue && t.DueDate < now)
            .ToListAsync(stoppingToken);

        if (overdueTasks.Count > 0)
        {
            foreach (var task in overdueTasks)
            {
                task.IsOverdue = true;
            }

            var updatedCount = await dbContext.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Marked {Count} tasks as overdue", updatedCount);
        }
        else
        {
            _logger.LogDebug("No overdue tasks found");
        }
    }
}

public class BackgroundServiceSettings
{
    public int CheckIntervalMinutes { get; set; } = 1;
    public int InitialDelaySeconds { get; set; } = 10;
}