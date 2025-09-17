using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// Validates that the password matches a specified regular expression pattern.
    /// </summary>
    public class RegexPasswordValidator : IPasswordValidator<TestAppIdentityUser>
    {
        private readonly Regex _pattern;
        private readonly string _description;

        public RegexPasswordValidator(string pattern, string description)
        {
            _pattern = new Regex(pattern);
            _description = description;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user, string password)
        {
            if (!_pattern.IsMatch(password))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordPatternMismatch",
                    Description = _description
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
