using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token from a ClaimsPrincipal.
        /// </summary>
        string GenerateToken(ClaimsPrincipal principal, DateTime expiresAt);

        /// <summary>
        /// Parses a JWT token and returns the ClaimsPrincipal.
        /// </summary>
        ClaimsPrincipal ValidateToken(string token);

        /// <summary>
        /// Returns the expiration time for a new token.
        /// </summary>
        DateTime GetExpiration();
    }
}
