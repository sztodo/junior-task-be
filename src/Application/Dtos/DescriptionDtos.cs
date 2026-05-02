using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record GenerateDescriptionRequest(
    [Required] string Name,
    [Required] string Manufacturer,
    [Required] string Type,
    [Required] string OperatingSystem,
    string? OsVersion,
    [Required] string Processor,
    [Required] int RamAmount
);

public record GenerateDescriptionResponse(string Description);

