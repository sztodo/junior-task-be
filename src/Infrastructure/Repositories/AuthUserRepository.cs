using Domain.Interfaces;
using Domain.Models.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuthUserRepository : IAuthUserRepository
{
    private readonly DeviceManagementDbContext _context;

    public AuthUserRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<AuthUser?> GetByEmailAsync(string email)
    {
        return await _context.AuthUsers
            .FirstOrDefaultAsync(a => a.Email == email && !a.IsDeleted);
    }

    public async Task<AuthUser?> GetByIdAsync(int id)
    {
        return await _context.AuthUsers.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<AuthUser> CreateAsync(AuthUser authUser)
    {
        _context.AuthUsers.Add(authUser);
        await _context.SaveChangesAsync();
        return authUser;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.AuthUsers.AnyAsync(a => a.Email == email);
    }

    public async Task DeleteAsync(AuthUser entity)
    {
        entity.DecomissionUser();
        _context.AuthUsers.Update(entity);
        await _context.SaveChangesAsync();
    }

    //not a needed implementation yet
    public Task<AuthUser> UpdateAsync(AuthUser entity)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<AuthUser>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
