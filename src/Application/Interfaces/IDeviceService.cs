using Application.Dtos;

namespace Application.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<DeviceDto>> GetAllAsync();
    Task<DeviceDto?> GetByIdAsync(int id);
    Task<DeviceDto> CreateAsync(CreateDeviceDto dto);
    Task<DeviceDto> UpdateAsync(int id, UpdateDeviceDto dto);
    Task DeleteAsync(int id);
    Task<DeviceDto> AssignUserAsync(int deviceId, AssignDeviceDto dto);
    Task<IEnumerable<DeviceDto>> GetByUserIdAsync(int userId);
    Task<DeviceDto> SelfAssignAsync(int deviceId, int userId);
    Task<DeviceDto> SelfUnassignAsync(int deviceId, int userId);
}
