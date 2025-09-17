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
    /// Validates that the username is not part of a reserved list (e.g., 'admin', 'root').
    /// </summary>
    public class ReservedUserNameValidator : IUserValidator<TestAppIdentityUser>
    {
        private static readonly HashSet<string> ReservedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin", "root", "system"
    };

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            if (ReservedNames.Contains(user.UserName ?? ""))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "ReservedUserName",
                    Description = $"Username '{user.UserName}' is reserved and cannot be used."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}

