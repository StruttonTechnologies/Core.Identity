using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    /// <summary>
    /// Represents the result of a successful login attempt, internal or external.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record LogoutRequestDto(
        string UserId, 
        string? RefreshToken = null
    );
}

