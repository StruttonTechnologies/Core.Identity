using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    /// <summary>
    /// Represents the response returned after a successful login or token issuance.
    /// </summary>
    public record LoginResponseDto(bool IsAuthenticated, string Token, DateTime ExpiresAt, string[] Roles, Guid UserId, string Username );
}
