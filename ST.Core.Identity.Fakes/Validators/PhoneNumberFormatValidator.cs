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
    /// Validates that the phone number matches a basic numeric format or E.164 pattern.
    /// </summary>
    public class PhoneNumberFormatValidator : IUserValidator<TestAppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneNumber) || !Regex.IsMatch(user.PhoneNumber, @"^\+?[1-9]\d{9,14}$"))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPhoneFormat",
                    Description = "Phone number must be in valid international format."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
