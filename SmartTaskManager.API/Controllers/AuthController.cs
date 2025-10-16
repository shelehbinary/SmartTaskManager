using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskManager.Core.DTOs;
using SmartTaskManager.Core.Services;

namespace SmartTaskManager.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(UserRegisterDto registerDto)
    {
        try
        {
            _logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);

            if (!ModelState.IsValid)
            {
                return Error<AuthResponseDto>("Invalid registration data");
            }

            var result = await _authService.RegisterAsync(registerDto);

            _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
            return Success(result, "User registered successfully");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registration failed: {Message}", ex.Message);
            return Error<AuthResponseDto>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for {Email}", registerDto.Email);
            return Error<AuthResponseDto>("An unexpected error occurred during registration");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(UserLoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            if (!ModelState.IsValid)
            {
                return Error<AuthResponseDto>("Invalid login data");
            }

            var result = await _authService.LoginAsync(loginDto);

            _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
            return Success(result, "Login successful");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login failed for {Email}: {Message}", loginDto.Email, ex.Message);
            return Unauthorized<AuthResponseDto>("Invalid email or password");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for {Email}", loginDto.Email);
            return Error<AuthResponseDto>("An unexpected error occurred during login");
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetCurrentUser()
    {
        try
        {
            var user = await _authService.GetCurrentUserAsync(CurrentUserId);
            return Success(user, "User data retrieved successfully");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("User not found: {UserId}", CurrentUserId);
            return NotFound<UserDto>("User not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user data for {UserId}", CurrentUserId);
            return Error<UserDto>("Error retrieving user data");
        }
    }
}