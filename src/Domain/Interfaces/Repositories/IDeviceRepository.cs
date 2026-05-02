using Domain.Models;

namespace Domain.Interfaces;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetAllWithUsersAsync();
    Task<Device?> GetByIdWithUserAsync(int id);
    Task<IEnumerable<Device>> GetByUserIdAsync(int userId);
    Task AssignUserAsync(int deviceId, int? userId);
}
