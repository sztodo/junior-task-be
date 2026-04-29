using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceManagementDbContext _context;

    public DeviceRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.Devices.Where(d => !d.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetAllWithUsersAsync()
    {
        return await _context.Devices
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .Include(d => d.AssignedUser)
            .ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(int id)
    {
        return await _context.Devices.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == id);
    }

    public async Task<Device?> GetByIdWithUserAsync(int id)
    {
        return await _context.Devices
            .Include(d => d.AssignedUser)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
    }

    public async Task<IEnumerable<Device>> GetByUserIdAsync(int userId)
    {
        return await _context.Devices
            .AsNoTracking()
            .Include(d => d.AssignedUser)
            .Where(d => d.AssignedUserId == userId && !d.IsDeleted)
            .ToListAsync();
    }

    public async Task<Device> CreateAsync(Device entity)
    {
        _context.Devices.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Device> UpdateAsync(Device entity)
    {
        _context.Devices.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Device device)
    {
        device.MarkAsDeleted();
        _context.Devices.Update(device);
        await _context.SaveChangesAsync();
    }

    //will sent the full entity or find a way to not do a get in an update repository method
    public async Task AssignUserAsync(int deviceId, int? userId)
    {
        var device = await _context.Devices.FindAsync(deviceId)
            ?? throw new KeyNotFoundException($"Device with ID {deviceId} not found.");
        device.AssignedUserId = userId;
        device.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
