using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record RegisterRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password,
    [Required] string Location,
    [Required] string Name,
    [Required] string Role
);

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

public record AuthResponse(
    string Token,
    string Email,
    string Role,
    int UserId,
    int AuthUserId
);
