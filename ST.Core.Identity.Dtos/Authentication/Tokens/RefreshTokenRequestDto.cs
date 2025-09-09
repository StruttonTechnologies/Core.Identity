using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    /// <summary>
    /// Represents the data required to request a new access token using a refresh token.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record RefreshTokenRequestDto(string RefreshToken);
}
