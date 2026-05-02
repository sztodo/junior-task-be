using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllWithDevicesAsync();
    Task<User?> GetByIdWithDevicesAsync(int id);
    Task<User?> GetByPropertiesAsync(string name, string role, string location);
}
