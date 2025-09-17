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
    /// Validates that the password does not contain leading, trailing, or internal whitespace.
    /// </summary>
    public class NoWhitespacePasswordValidator : IPasswordValidator<TestAppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Contains(" "))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsWhitespace",
                    Description = "Password must not contain whitespace."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }

}
