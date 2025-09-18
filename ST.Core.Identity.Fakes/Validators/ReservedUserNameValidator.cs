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
        private readonly HashSet<string> _reservedNames;
        private readonly string _description;

        public ReservedUserNameValidator(IEnumerable<string> reservedNames, string description)
        {
            _reservedNames = new HashSet<string>(reservedNames, StringComparer.OrdinalIgnoreCase);
            _description = description;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            if (_reservedNames.Contains(user.UserName ?? ""))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "ReservedUserName",
                    Description = _description
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}

