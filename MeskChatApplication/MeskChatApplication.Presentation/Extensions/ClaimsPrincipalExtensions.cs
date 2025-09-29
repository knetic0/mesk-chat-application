using System.Security.Claims;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetNameIdentifier(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out var guid)) throw new UnauthorizedAccessException();
        return guid;
    }
}