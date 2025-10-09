using ST.Core.Identity.Dtos.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Users.Mapping
{
    public static class ClaimsIdentityExtensions
    {
        public static ClaimsPrincipalDto ToClaimsPrincipalDto(this ClaimsIdentity identity)
        {
            var claims = identity.Claims
                .Select(c => new ClaimDto(c.Type, c.Value))
                .ToList();

            return new ClaimsPrincipalDto(
                identity.AuthenticationType,
                identity.IsAuthenticated,
                identity.Name,
                claims
            );
        }
    }
}
