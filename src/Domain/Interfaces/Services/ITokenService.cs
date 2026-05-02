using Domain.Models.Entities;

namespace Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(AuthUser user);
}
