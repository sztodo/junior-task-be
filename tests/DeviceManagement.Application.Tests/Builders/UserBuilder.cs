using Domain.Models;

namespace DeviceManagement.Application.Tests.Builders;

public class UserBuilder
{
    private int _id = 1;
    private string _name = "Test User";
    private string _role = "Engineer";
    private string _location = "Test City";

    public static UserBuilder Default() => new();

    public UserBuilder WithId(int id) { _id = id; return this; }
    public UserBuilder WithName(string name) { _name = name; return this; }

    public User Build() => new()
    {
        Id = _id,
        Name = _name,
        Role = _role,
        Location = _location,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

}
