using System;
using System.Collections.Generic;

namespace ST.Core.Identity.IdentitySeed
{
    /// <summary>
    /// Centralized seed data for identity-related validation.
    /// </summary>
    public static class IdentitySeed
    {
        /// <summary>
        /// Supported identity providers for authentication.
        /// </summary>
        public static readonly HashSet<string> KnownIdentityProviders = new(StringComparer.OrdinalIgnoreCase)
        {
            "Google", "Microsoft", "GitHub", "Okta", "Auth0"
        };

        /// <summary>
        /// Allowed roles for access control.
        /// </summary>
        public static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            "User", "Admin", "Manager", "Support", "Contributor"
        };

        /// <summary>
        /// Authorized scopes for token issuance and access boundaries.
        /// </summary>
        public static readonly HashSet<string> AllowedScopes = new(StringComparer.OrdinalIgnoreCase)
        {
            "read:user",
            "write:profile",
            "admin:tenant",
            "read:settings",
            "manage:roles"
        };

        /// <summary>
        /// Reserved usernames that cannot be claimed by users.
        /// </summary>
        public static readonly HashSet<string> ReservedUsernames = new(StringComparer.OrdinalIgnoreCase)
        {
            "admin", "root", "system", "support", "me"
        };
    }
}