using System;
using System.Security.Claims;

namespace ST.Core.Identity.Application.Authentication.Interfaces
{
    /// <summary>
    /// Defines operations for generating and validating JWT tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token from the provided ClaimsPrincipal using configured options.
        /// </summary>
        /// <param name="principal">The authenticated user principal.</param>
        /// <returns>A signed JWT token string.</returns>
        string GenerateToken(ClaimsPrincipal principal);

        /// <summary>
        /// Validates a JWT token and returns the associated ClaimsPrincipal, or null if invalid.
        /// </summary>
        /// <param name="token">The JWT token string to validate.</param>
        /// <returns>The ClaimsPrincipal if valid; otherwise, null.</returns>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Extracts the expiration timestamp from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token string.</param>
        /// <returns>The expiration DateTime if available; otherwise, null.</returns>
        DateTime? GetExpiration(string token);
    }
}