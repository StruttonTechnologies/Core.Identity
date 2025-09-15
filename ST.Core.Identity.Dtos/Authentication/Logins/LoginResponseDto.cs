using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    /// <summary>
    /// Represents the response returned after a successful login or token issuance.
    /// </summary>
    //public record LoginResponseDto(string Token, DateTime ExpiresAt, string[] Roles, Guid UserId, string Username );
    /// <summary>
    /// Represents the result of a successful login attempt, internal or external.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record LoginResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        Guid UserId,
        string Username,
        string Provider,
        bool IsNewUser = false,
        bool RequiresTwoFactor = false
    );
}
