using System;
using System.Collections.Generic;

namespace ST.Core.Identity.Data
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
            "Google", 
            "Microsoft", 
            "GitHub", 
            "Okta", 
            "Auth0", 
            "Local"
            // More can be added as needed
        };

        /// <summary>
        /// Allowed roles for access control.
        /// </summary>
        public static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            "User", 
            "Admin", 
            "Manager", 
            "Support", 
            "Contributor"
            // More can be added as needed
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
            // More can be added as needed
        };

        /// <summary>
        /// Reserved usernames that cannot be claimed by users.
        /// </summary>
        public static readonly HashSet<string> ReservedUsernames = new(StringComparer.OrdinalIgnoreCase)
        {
            "admin", 
            "root", 
            "system", 
            "support", 
            "me"
            // More can be added as needed
        };

        
        /// <summary>
        /// Commonly blacklisted passwords and terms that are not allowed for security reasons.
        /// </summary>
        public static readonly HashSet<string> Blacklist = new(StringComparer.OrdinalIgnoreCase)
        {
            "password",
            "123456",
            "qwerty",
            "letmein",
            "welcome",
            "admin",
            "iloveyou",
            "monkey",
            "abc123"
            // More can be added as needed
        };
    }
}