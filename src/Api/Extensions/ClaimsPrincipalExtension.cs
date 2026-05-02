using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue("linked_user_id");

        return int.TryParse(value, out var id) && id > 0
            ? id
            : throw new UnauthorizedAccessException(
                "No company user record is linked to your account. Contact an administrator.");

    }

    public static int GetAuthUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User identity not found in token.");

        return int.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("User ID claim is not a valid integer.");
    }
}
