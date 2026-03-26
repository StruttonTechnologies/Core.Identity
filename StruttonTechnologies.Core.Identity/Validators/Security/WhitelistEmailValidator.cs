using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a user's email address is whitelisted.
    /// </summary>
    /// <typeparam name="TUser">The user type, which must inherit from IdentityUser&lt;Guid&gt;.</typeparam>
    public class WhitelistEmailValidator<TUser> : IUserValidator<TUser>
        where TUser : IdentityUser<Guid>
    {
        private static readonly HashSet<string> WhitelistedEmails = new(StringComparer.OrdinalIgnoreCase)
        {
            "admin@struttontechnologies.com",
            "support@struttontechnologies.com"
        };

        private static readonly HashSet<string> WhitelistedDomains = new(StringComparer.OrdinalIgnoreCase)
        {
            "struttontechnologies.com",
            "example.com"
        };

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            string? email = user?.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            if (WhitelistedEmails.Contains(email))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            string? domain = email.Contains('@', StringComparison.Ordinal) ? email.Split('@')[1] : null;

            if (!string.IsNullOrWhiteSpace(domain) && WhitelistedDomains.Contains(domain))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            IdentityError error = new IdentityError
            {
                Code = "EmailNotWhitelisted",
                Description = "Email address is not allowed."
            };

            return Task.FromResult(IdentityResult.Failed(error));
        }
    }
}