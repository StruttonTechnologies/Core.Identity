using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents the data required to add a claim to a user.
    /// </summary>
    ///  [ExcludeFromCodeCoverage]
    public record AddClaimRequestDto(Guid UserId,string ClaimType,string ClaimValue);
}
