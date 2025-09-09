using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.User
{

    /// <summary>
    /// Represents the identity details of an authenticated user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record AuthenticatedUserDto(
        string UserId,
        string Username,
        string Email,
        string[] Roles,
        bool IsTwoFactorEnabled
    );
}
