using Domain.Models;
using Domain.Models.Enums;

namespace DeviceManagement.Application.Tests.Builders;

public class DeviceBuilder
{
    private int _id = 1;
    private string _name = "Test Phone";
    private string _manufacturer = "TestCo";
    private DeviceType _type = DeviceType.Phone;
    private string _os = "Android";
    private string _osVersion = "14";
    private string _processor = "Snapdragon 888";
    private int _ram = 8;
    private string? _description = null;
    private int? _assignedUserId = null;

    public static DeviceBuilder Default() => new();

    public DeviceBuilder WithId(int id) { _id = id; return this; }
    public DeviceBuilder WithName(string name) { _name = name; return this; }
    public DeviceBuilder WithManufacturer(string mfr) { _manufacturer = mfr; return this; }
    public DeviceBuilder WithType(DeviceType type) { _type = type; return this; }
    public DeviceBuilder WithRam(int ram) { _ram = ram; return this; }
    public DeviceBuilder WithAssignedUser(int userId) { _assignedUserId = userId; return this; }
    public DeviceBuilder Unassigned() { _assignedUserId = null; return this; }

    public Device Build() => new()
    {
        Id = _id,
        Name = _name,
        Manufacturer = _manufacturer,
        Type = _type,
        OperatingSystem = _os,
        OsVersion = _osVersion,
        Processor = _processor,
        RamAmount = _ram,
        Description = _description,
        AssignedUserId = _assignedUserId,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}
