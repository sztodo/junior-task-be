using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;

    public DeviceService(IDeviceRepository deviceRepository, IUserRepository userRepository)
    {
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllWithUsersAsync();
        return devices.Select(MapToDto);
    }

    public async Task<DeviceDto?> GetByIdAsync(int id)
    {
        var device = await _deviceRepository.GetByIdWithUserAsync(id);
        return device is null ? null : MapToDto(device);
    }

    public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto)
    {
        if (dto.AssignedUserId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(dto.AssignedUserId.Value)
                ?? throw new KeyNotFoundException($"User with ID {dto.AssignedUserId} not found.");
        }

        var device = new Device
        {
            Name = dto.Name,
            Manufacturer = dto.Manufacturer,
            Type = dto.Type,
            OperatingSystem = dto.OperatingSystem,
            OsVersion = dto.OsVersion,
            Processor = dto.Processor,
            RamAmount = dto.RamAmount,
            Description = dto.Description,
            AssignedUserId = dto.AssignedUserId
        };

        var created = await _deviceRepository.CreateAsync(device);
        var withUser = await _deviceRepository.GetByIdWithUserAsync(created.Id);
        return MapToDto(withUser!);
    }

    public async Task<DeviceDto> UpdateAsync(int id, UpdateDeviceDto dto)
    {
        var device = await _deviceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Device with ID {id} not found.");

        if (dto.AssignedUserId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(dto.AssignedUserId.Value)
                ?? throw new KeyNotFoundException($"User with ID {dto.AssignedUserId} not found.");
        }

        device.Name = dto.Name;
        device.Manufacturer = dto.Manufacturer;
        device.Type = dto.Type;
        device.OperatingSystem = dto.OperatingSystem;
        device.OsVersion = dto.OsVersion;
        device.Processor = dto.Processor;
        device.RamAmount = dto.RamAmount;
        device.Description = dto.Description;
        device.AssignedUserId = dto.AssignedUserId;
        device.UpdatedAt = DateTime.UtcNow;

        await _deviceRepository.UpdateAsync(device);
        var updated = await _deviceRepository.GetByIdWithUserAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var device = await _deviceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Device with ID {id} not found.");

        await _deviceRepository.DeleteAsync(device);
    }

    public async Task<DeviceDto> AssignUserAsync(int deviceId, AssignDeviceDto dto)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId)
            ?? throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

        if (dto.UserId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId.Value)
                ?? throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
        }

        await _deviceRepository.AssignUserAsync(deviceId, dto.UserId);
        var updated = await _deviceRepository.GetByIdWithUserAsync(deviceId);
        return MapToDto(updated!);
    }

    public async Task<IEnumerable<DeviceDto>> GetByUserIdAsync(int userId)
    {
        var devices = await _deviceRepository.GetByUserIdAsync(userId);
        return devices.Select(MapToDto);
    }

    private static DeviceDto MapToDto(Device d) => new(
        d.Id,
        d.Name,
        d.Manufacturer,
        d.Type.ToString(),
        d.OperatingSystem,
        d.OsVersion,
        d.Processor,
        d.RamAmount,
        d.Description,
        d.AssignedUserId,
        d.AssignedUser?.Name,
        d.CreatedAt,
        d.UpdatedAt
    );

}
