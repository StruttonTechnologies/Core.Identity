using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// Validates that the email domain is part of an approved whitelist.
    /// </summary>
    public class DomainWhitelistEmailValidator : IUserValidator<TestAppIdentityUser>
    {
        private readonly HashSet<string> _allowedDomains;

        public DomainWhitelistEmailValidator(IEnumerable<string> allowedDomains)
        {
            _allowedDomains = new HashSet<string>(allowedDomains, StringComparer.OrdinalIgnoreCase);
        }

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            var domain = user.Email?.Split('@').LastOrDefault();
            if (domain == null || !_allowedDomains.Contains(domain))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailDomainNotAllowed",
                    Description = $"Email domain '{domain}' is not permitted."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
