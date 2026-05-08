namespace Application.Dtos;

public record DeviceSearchResultDto(
    int Id,
    string Name,
    string Manufacturer,
    string TypeLabel,
    string OperatingSystem,
    string OsVersion,
    string Processor,
    int RamAmount,
    string? Description,
    int? AssignedUserId,
    string? AssignedUserName,
    int Score
);

public record DeviceSearchRequest(string Query);
