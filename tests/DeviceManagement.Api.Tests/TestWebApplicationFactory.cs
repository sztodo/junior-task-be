
using System.Data.Common;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Enums;
using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Api.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private DbConnection _connection;
    public TestWebApplicationFactory()
    {
        // 1. Creăm conexiunea manual și o deschidem
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddInMemoryCollection(new Dictionary<string, string?>
            {
            { "JwtSettings:SecretKey", "O-Cheie-Super-Secret-Si-Lunga-De-Minim-32-Caractere-123!" },
            { "JwtSettings:Issuer", "DeviceManagement" },
            { "JwtSettings:Audience", "DeviceManagement" },
            { "ConnectionStrings:DefaultConnection", "DataSource=:memory:" }
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(
                d => d.ServiceType.Name.Contains("DbContextOptions") ||
                     d.ServiceType == typeof(DeviceManagementDbContext)).ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            services.AddDbContext<DeviceManagementDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DeviceManagementDbContext>();

            db.Database.EnsureCreated();
            var seeder = scope.ServiceProvider;
            SeedTestData(db, seeder);
        });
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }

    private static void SeedTestData(DeviceManagementDbContext db, IServiceProvider services)
    {
        if (db.AuthUsers.Any()) return;

        var hasher = services.GetRequiredService<IPasswordHasher>();

        db.Users.AddRange(
            new User
            {
                Id = 1,
                Name = "Alice Johnson",
                Role = "Engineer",
                Location = "London",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                Name = "Bob Smith",
                Role = "QA",
                Location = "Berlin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        db.Devices.AddRange(
            new Device
            {
                Id = 1,
                Name = "iPhone 15 Pro",
                Manufacturer = "Apple",
                Type = DeviceType.Phone,
                OperatingSystem = "iOS",
                OsVersion = "17.2",
                Processor = "A17 Pro",
                RamAmount = 8,
                AssignedUserId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Device
            {
                Id = 2,
                Name = "Galaxy S24",
                Manufacturer = "Samsung",
                Type = DeviceType.Phone,
                OperatingSystem = "Android",
                OsVersion = "14",
                Processor = "Snapdragon 8 Gen 3",
                RamAmount = 12,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        db.AuthUsers.AddRange(
            new AuthUser
            {
                Id = 1,
                Email = "alice@example.com",
                PasswordHash = hasher.Hash("Password123!"),
                Role = AuthRoles.Employee,
                LinkedUserId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new AuthUser
            {
                Id = 2,
                Email = "admin@example.com",
                PasswordHash = hasher.Hash("Password123!"),
                Role = AuthRoles.Admin,
                LinkedUserId = 2,
                CreatedAt = DateTime.UtcNow
            }
        );

        db.SaveChanges();
    }
}
