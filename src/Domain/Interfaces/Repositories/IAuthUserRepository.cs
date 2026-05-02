using Domain.Models.Entities;

namespace Domain.Interfaces;

public interface IAuthUserRepository : IRepository<AuthUser>
{
    Task<AuthUser?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}
