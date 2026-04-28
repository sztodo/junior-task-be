using Domain.Models.Enums;

namespace Application.Dtos;

public record DeviceDto(
    int Id,
    string Name,
    string Manufacturer,
    DeviceType Type,
    string TypeLabel,
    string OperatingSystem,
    string OsVersion,
    string Processor,
    int RamAmount,
    string? Description,
    int? AssignedUserId,
    string? AssignedUserName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateDeviceDto(
    string Name,
    string Manufacturer,
    DeviceType Type,
    string OperatingSystem,
    string OsVersion,
    string Processor,
    int RamAmount,
    string? Description,
    int? AssignedUserId
);

public record UpdateDeviceDto(
    string Name,
    string Manufacturer,
    DeviceType Type,
    string OperatingSystem,
    string OsVersion,
    string Processor,
    int RamAmount,
    string? Description,
    int? AssignedUserId
);

public record AssignDeviceDto(int? UserId);