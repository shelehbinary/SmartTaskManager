using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskManager.Core.DTOs;
using SmartTaskManager.Core.Services;

namespace SmartTaskManager.API.Controllers;

[Authorize]
public class TasksController : BaseApiController
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TaskResponseDto>>>> GetTasks()
    {
        try
        {
            var tasks = await _taskService.GetUserTasksAsync(CurrentUserId);
            return Success(tasks, $"Retrieved {tasks.Count()} tasks");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks for user {UserId}", CurrentUserId);
            return Error<IEnumerable<TaskResponseDto>>("Error retrieving tasks");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> GetTask(Guid id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(CurrentUserId, id);

            if (task == null)
            {
                return NotFound<TaskResponseDto>("Task not found");
            }

            return Success(task, "Task retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task {TaskId} for user {UserId}", id, CurrentUserId);
            return Error<TaskResponseDto>("Error retrieving task");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> CreateTask(TaskCreateDto taskDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Error<TaskResponseDto>("Invalid task data");
            }

            var task = await _taskService.CreateTaskAsync(CurrentUserId, taskDto);

            _logger.LogInformation("Task created: {TaskId} for user {UserId}", task.Id, CurrentUserId);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task, "Task created successfully");
        }
        catch (ArgumentException ex)
        {
            return Error<TaskResponseDto>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task for user {UserId}", CurrentUserId);
            return Error<TaskResponseDto>("Error creating task");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> UpdateTask(Guid id, TaskUpdateDto taskDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Error<TaskResponseDto>("Invalid task data");
            }

            var task = await _taskService.UpdateTaskAsync(CurrentUserId, id, taskDto);
            return Success(task, "Task updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound<TaskResponseDto>("Task not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {TaskId} for user {UserId}", id, CurrentUserId);
            return Error<TaskResponseDto>("Error updating task");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTask(Guid id)
    {
        try
        {
            var result = await _taskService.DeleteTaskAsync(CurrentUserId, id);

            if (!result)
            {
                return NotFound<bool>("Task not found");
            }

            _logger.LogInformation("Task deleted: {TaskId} by user {UserId}", id, CurrentUserId);
            return Success(true, "Task deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId} for user {UserId}", id, CurrentUserId);
            return Error<bool>("Error deleting task");
        }
    }

    [HttpGet("dashboard/stats")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
    {
        try
        {
            var stats = await _taskService.GetDashboardStatsAsync(CurrentUserId);
            return Success(stats, "Dashboard stats retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard stats for user {UserId}", CurrentUserId);
            return Error<DashboardStatsDto>("Error retrieving dashboard statistics");
        }
    }
}