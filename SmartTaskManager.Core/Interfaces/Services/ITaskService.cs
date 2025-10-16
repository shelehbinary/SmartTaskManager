using SmartTaskManager.Core.DTOs;

namespace SmartTaskManager.Core.Services;

public interface ITaskService
{
    Task<TaskResponseDto> CreateTaskAsync(Guid userId, TaskCreateDto taskDto);
    Task<IEnumerable<TaskResponseDto>> GetUserTasksAsync(Guid userId);
    Task<TaskResponseDto?> GetTaskByIdAsync(Guid userId, Guid taskId);
    Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, TaskUpdateDto taskDto);
    Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);
    Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId);
}
