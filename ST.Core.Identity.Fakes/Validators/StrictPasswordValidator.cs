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
    /// Validates that the password meets strict requirements: minimum length, digit, uppercase, lowercase, and special character.
    /// </summary>
    public class StrictPasswordValidator : IPasswordValidator<TestAppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                errors.Add(new IdentityError { Code = "PasswordTooShort", Description = "Password must be at least 8 characters long." });

            if (!password.Any(char.IsDigit))
                errors.Add(new IdentityError { Code = "PasswordMissingDigit", Description = "Password must contain at least one digit." });

            if (!password.Any(char.IsUpper))
                errors.Add(new IdentityError { Code = "PasswordMissingUppercase", Description = "Password must contain at least one uppercase letter." });

            if (!password.Any(char.IsLower))
                errors.Add(new IdentityError { Code = "PasswordMissingLowercase", Description = "Password must contain at least one lowercase letter." });

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add(new IdentityError { Code = "PasswordMissingSpecial", Description = "Password must contain at least one special character." });

            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}

