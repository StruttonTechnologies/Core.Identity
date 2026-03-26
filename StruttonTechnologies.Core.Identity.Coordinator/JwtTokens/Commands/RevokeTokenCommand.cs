using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Commands
{
    /// <summary>
    /// Command to revoke JWT tokens. Can revoke a specific token or all tokens for a user.
    /// </summary>
    public sealed record RevokeTokenCommand : IRequest<Unit>
    {
        /// <summary>
        /// The specific token to revoke (optional if UserId is provided).
        /// </summary>
        public string? Token { get; init; }

        /// <summary>
        /// The user ID to revoke all tokens for (optional if Token is provided).
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Indicates whether the token is a refresh token (true) or access token (false).
        /// Only used when Token is provided.
        /// </summary>
        public bool IsRefreshToken { get; init; }

        public RevokeTokenCommand(string? token = null, string? userId = null, bool isRefreshToken = false)
        {
            Token = token;
            UserId = userId;
            IsRefreshToken = isRefreshToken;
        }
    }
}
