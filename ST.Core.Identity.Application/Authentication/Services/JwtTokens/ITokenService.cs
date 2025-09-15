using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.Tokens
{
    public interface ITokenService
    {
        string GenerateToken(ClaimsPrincipal principal);
        DateTime GetExpirationTime();

        /// <summary>
        /// Revokes a token by storing its identifier or associated user in a denylist.
        /// </summary>
        void RevokeToken(string token);

        /// <summary>
        /// Checks whether a token has been revoked.
        /// </summary>
        bool IsTokenRevoked(string token);
    }
}
