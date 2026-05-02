using Domain.Models.Enums;

namespace Domain.Models.Entities;

public class AuthUser
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = AuthRoles.Employee;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation
    public int? LinkedUserId { get; set; }
    public User? LinkedUser { get; set; }

    public void DecomissionUser()
    {
        var uniqueSuffix = DateTime.UtcNow.Ticks;
        Email = $"decomissioned_{uniqueSuffix}_{Email}";
        DeletedAt = DateTime.Now;
        IsDeleted = true;
        LinkedUser = null;
        LinkedUserId = null;
        PasswordHash = string.Empty;
    }
}
