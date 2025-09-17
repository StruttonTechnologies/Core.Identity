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
    /// Validates that the phone number is not part of a known blacklist.
    /// </summary>
    public class BlacklistPhoneValidator : IUserValidator<TestAppIdentityUser>
    {
        private static readonly HashSet<string> Blacklist = new()
    {
        "+10000000000", "+19999999999"
    };

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            if (Blacklist.Contains(user.PhoneNumber ?? ""))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PhoneNumberBlacklisted",
                    Description = "Phone number is not allowed."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }


}
