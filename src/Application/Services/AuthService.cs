using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Enums;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IAuthUserRepository authUserRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _authUserRepository = authUserRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _authUserRepository.ExistsByEmailAsync(request.Email))
            throw new InvalidOperationException($"An account with email '{request.Email}' already exists.");
        var user = await _userRepository.GetByPropertiesAsync(request.Name, request.Role, request.Location);
        var userId = user?.Id;
        if (user == null)
        {
            user = new User
            {
                Name = request.Name,
                Role = request.Role,
                Location = request.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var createdUser = await _userRepository.CreateAsync(user);
            userId = createdUser.Id;
        }

        var authUser = new AuthUser
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = AuthRoles.Employee,
            CreatedAt = DateTime.UtcNow,
            LinkedUserId = userId,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _authUserRepository.CreateAsync(authUser);
        var token = _tokenService.GenerateToken(created);

        return new AuthResponse(token, created.Email, created.Role, created.Id, userId ?? 0);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var authUser = await _authUserRepository.GetByEmailAsync(request.Email.ToLowerInvariant())
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!_passwordHasher.Verify(request.Password, authUser.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = _tokenService.GenerateToken(authUser);

        return new AuthResponse(token, authUser.Email, authUser.Role, authUser.LinkedUserId ?? 0, authUser.Id);
    }
}
