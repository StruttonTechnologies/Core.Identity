using System.Security.Claims;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.Identity.Stub.Builders
{
    /// <summary>
    /// Builds stubbed <see cref="ClaimsPrincipal"/> instances for testing token generation and validation.
    /// </summary>
    public static class StubClaimsPrincipalBuilder
    {
        /// <summary>
        /// Creates a claims principal for the specified stub user.
        /// </summary>
        public static ClaimsPrincipal For(StubUser user)
        {
            var identity = new ClaimsIdentity("Stub");

            if (user.Id != Guid.Empty)
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(user.Email))
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            if (!string.IsNullOrWhiteSpace(user.DisplayName))
                identity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName));

            identity.AddClaim(new Claim("jti", Guid.NewGuid().ToString())); // ✅ Add JTI

            foreach (var role in user.Roles ?? Enumerable.Empty<string>())
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Creates a claims principal with the specified roles and optional user ID.
        /// </summary>
        public static ClaimsPrincipal WithRoles(params string[] roles)
        {
            return WithRoles(Guid.NewGuid(), roles);
        }

        /// <summary>
        /// Creates a claims principal with the specified user ID and roles.
        /// </summary>
        public static ClaimsPrincipal WithRoles(Guid userId, params string[] roles)
        {
            var identity = new ClaimsIdentity("Stub");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, "stub@example.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Stub User"));
            identity.AddClaim(new Claim("jti", Guid.NewGuid().ToString())); // ✅ Add JTI

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Creates an anonymous claims principal with no identity.
        /// </summary>
        public static ClaimsPrincipal Anonymous()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}