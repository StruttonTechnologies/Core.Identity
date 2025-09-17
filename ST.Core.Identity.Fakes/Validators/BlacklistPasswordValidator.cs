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
    /// Validates that the password is not part of a known blacklist of weak passwords.
    /// </summary>
    public class BlacklistPasswordValidator : IPasswordValidator<TestAppIdentityUser>
    {
        private static readonly HashSet<string> Blacklist = new(StringComparer.OrdinalIgnoreCase)
    {
        "password", "123456", "qwerty", "letmein", "admin"
    };

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user, string password)
        {
            if (Blacklist.Contains(password))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordBlacklisted",
                    Description = "Password is too common or insecure."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
