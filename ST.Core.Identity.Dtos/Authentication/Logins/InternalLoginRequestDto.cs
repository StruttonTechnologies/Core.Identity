using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    /// <summary>
    /// Represents the credentials submitted during an internal login attempt.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record InternalLoginRequestDto(
        string UsernameOrEmail,
        string Password
    );
}




