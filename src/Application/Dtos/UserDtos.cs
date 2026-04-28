namespace Application.Dtos;

public record UserDto(
    int Id,
    string Name,
    string Role,
    string Location,
    IEnumerable<DeviceSummaryDto> AssignedDevices,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record DeviceSummaryDto(int Id, string Name, string Manufacturer);

public record CreateUserDto(
    string Name,
    string Role,
    string Location
);

public record UpdateUserDto(
    string Name,
    string Role,
    string Location
);