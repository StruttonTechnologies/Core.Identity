using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Mapping
{
    public static class ClaimsIdentityExtensions
    {
        public static ClaimsPrincipalDto ToClaimsPrincipalDto(this ClaimsIdentity identity)
        {
            ArgumentNullException.ThrowIfNull(identity, nameof(identity));

            List<ClaimDto> claims = identity.Claims
                .Select(c => new ClaimDto(c.Type, c.Value))
                .ToList();

            return new ClaimsPrincipalDto(
                identity.AuthenticationType,
                identity.IsAuthenticated,
                identity.Name,
                claims);
        }
    }
}
