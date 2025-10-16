using SmartTaskManager.Core.DTOs;

namespace SmartTaskManager.Core.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
    Task<UserDto> GetCurrentUserAsync(Guid userId);
}
