using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
