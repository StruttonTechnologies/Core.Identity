using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents a request to register a new user.
    /// </summary>
    /// <param name="Email">The email address for the new user.</param>
    /// <param name="Password">The password for the new user.</param>
    /// <param name="ConfirmPassword">The optional password confirmation value.</param>
    [ExcludeFromCodeCoverage]

    public sealed record RegisterDto(string Email, string Password, string? ConfirmPassword = null);

    /// <summary>
    /// Represents a request to authenticate with an external identity provider.
    /// </summary>
    /// <param name="Provider">The external identity provider name.</param>
    /// <param name="IdToken">The provider-issued ID token.</param>
    [ExcludeFromCodeCoverage]

    public sealed record ExternalLoginDto(string Provider, string IdToken);

    /// <summary>
    /// Represents a request to exchange a refresh token for a new access token.
    /// </summary>
    /// <param name="RefreshToken">The refresh token value.</param>
    [ExcludeFromCodeCoverage]

    public sealed record RefreshTokenDto(string RefreshToken);

    /// <summary>
    /// Represents an issued token pair and expiration timestamp.
    /// </summary>
    /// <param name="AccessToken">The issued access token.</param>
    /// <param name="RefreshToken">The issued refresh token.</param>
    /// <param name="ExpiresAt">The access token expiration timestamp.</param>
    [ExcludeFromCodeCoverage]

    public sealed record TokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
