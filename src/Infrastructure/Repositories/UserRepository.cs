using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DeviceManagementDbContext _context;

    public UserRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllWithDevicesAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted)
            .Include(u => u.AssignedDevices)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == id);
    }

    public async Task<User?> GetByIdWithDevicesAsync(int id)
    {
        return await _context.Users
            .Include(u => u.AssignedDevices)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }

    public async Task<User> CreateAsync(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(User user)
    {
        user.MarkAsDeleted();
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
