using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllWithDevicesAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdWithDevicesAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Role = dto.Role,
            Location = dto.Location
        };

        var created = await _userRepository.CreateAsync(user);
        var withDevices = await _userRepository.GetByIdWithDevicesAsync(created.Id);
        return MapToDto(withDevices!);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");

        user.Name = dto.Name;
        user.Role = dto.Role;
        user.Location = dto.Location;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        var updated = await _userRepository.GetByIdWithDevicesAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");

        await _userRepository.DeleteAsync(user);
    }

    private static UserDto MapToDto(User u) => new(
        u.Id,
        u.Name,
        u.Role,
        u.Location,
        u.AssignedDevices.Select(d => new DeviceSummaryDto(d.Id, d.Name, d.Manufacturer)),
        u.CreatedAt,
        u.UpdatedAt
    );
}
