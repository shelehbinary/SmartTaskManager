using SmartTaskManager.Core.DTOs;
using SmartTaskManager.Core.Entities;
using SmartTaskManager.Core.Interfaces.Repositories;
using SmartTaskManager.Core.Services;

namespace SmartTaskManager.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, JwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerDto.Password, 13);

        var user = new User
        {
            Email = registerDto.Email.ToLower(),
            PasswordHash = passwordHash,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = _jwtService.GetTokenExpiration(token),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.Email.ToLower());
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = _jwtService.GetTokenExpiration(token),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}