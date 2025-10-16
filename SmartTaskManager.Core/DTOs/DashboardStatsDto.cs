namespace SmartTaskManager.Core.DTOs;

public class DashboardStatsDto
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int PendingTasks { get; set; }
    public double CompletionRate { get; set; }
}
