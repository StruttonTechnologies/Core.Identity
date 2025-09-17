using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators
{
    public class EmailFormatValidator : IUserValidator<TestAppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            var isValid = user.Email?.Contains("@") == true;
            if (!isValid)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "Email format is invalid."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }

}
