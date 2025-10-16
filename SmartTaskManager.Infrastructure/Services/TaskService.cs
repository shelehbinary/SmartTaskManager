using SmartTaskManager.Core.DTOs;
using SmartTaskManager.Core.Entities;
using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Interfaces.Repositories;
using SmartTaskManager.Core.Services;

namespace SmartTaskManager.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskResponseDto> CreateTaskAsync(Guid userId, TaskCreateDto taskDto)
    {
        if (taskDto.DueDate < DateTime.UtcNow.AddMinutes(-5))
        {
            throw new ArgumentException("Due date cannot be in the past");
        }

        var task = new TaskItem
        {
            Title = taskDto.Title.Trim(),
            Description = taskDto.Description?.Trim(),
            DueDate = taskDto.DueDate.ToUniversalTime(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.TaskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToTaskResponseDto(task);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetUserTasksAsync(Guid userId)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksByUserIdAsync(userId);
        return tasks.Select(MapToTaskResponseDto);
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(Guid userId, Guid taskId)
    {
        var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            return null;

        return MapToTaskResponseDto(task);
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, TaskUpdateDto taskDto)
    {
        var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            throw new KeyNotFoundException("Task not found");

        if (taskDto.IsCompleted && !task.IsCompleted)
        {
            task.CompletedAt = DateTime.UtcNow;
        }
        else if (!taskDto.IsCompleted && task.IsCompleted)
        {
            task.CompletedAt = null;
        }

        task.Title = taskDto.Title.Trim();
        task.Description = taskDto.Description?.Trim();
        task.DueDate = taskDto.DueDate.ToUniversalTime();
        task.IsCompleted = taskDto.IsCompleted;

        _unitOfWork.TaskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToTaskResponseDto(task);
    }

    public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
    {
        var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            return false;

        _unitOfWork.TaskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksByUserIdAsync(userId);
        var taskList = tasks.ToList();

        var stats = new DashboardStatsDto
        {
            TotalTasks = taskList.Count,
            CompletedTasks = taskList.Count(t => t.IsCompleted),
            OverdueTasks = taskList.Count(t => t.IsOverdue),
            PendingTasks = taskList.Count(t => !t.IsCompleted)
        };

        stats.CompletionRate = stats.TotalTasks > 0
            ? Math.Round((double)stats.CompletedTasks / stats.TotalTasks * 100, 2)
            : 0;

        return stats;
    }

    private static TaskResponseDto MapToTaskResponseDto(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            IsOverdue = task.IsOverdue,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }
}
