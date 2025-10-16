using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartTaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }
    }

    protected ActionResult<ApiResponse<T>> Success<T>(T data, string message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
        return Ok(response);
    }

    protected ActionResult<ApiResponse<T>> Created<T>(string uri, T data, string message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
        return base.Created(uri, response);
    }

    protected ActionResult<ApiResponse<T>> CreatedAtAction<T>(string actionName, object routeValues, T data, string message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
        return base.CreatedAtAction(actionName, routeValues, response);
    }

    protected ActionResult<ApiResponse<T>> Error<T>(string message, int statusCode = 400)
    {
        var response = new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
        return StatusCode(statusCode, response);
    }

    protected ActionResult<ApiResponse<T>> NotFound<T>(string message = "Resource not found")
    {
        return Error<T>(message, 404);
    }

    protected ActionResult<ApiResponse<T>> Unauthorized<T>(string message = "Unauthorized")
    {
        return Error<T>(message, 401);
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}