using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.API.Controllers;

public class HealthController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetHealth()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();

            var healthInfo = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Database = canConnect ? "Connected" : "Disconnected",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            };

            _logger.LogInformation("Health check executed - Status: {Status}", healthInfo.Status);

            return Success<object>(healthInfo, "API is healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");

            var healthInfo = new
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Database = "Error",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Error = ex.Message
            };

            return StatusCode(503, new ApiResponse<object>
            {
                Success = false,
                Message = "API is unhealthy",
                Data = healthInfo
            });
        }
    }
}