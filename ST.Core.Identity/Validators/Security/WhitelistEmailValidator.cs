using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Security
{
    using Microsoft.AspNetCore.Identity;

    namespace ST.Core.Identity.Validators.Security
    {
        /// <summary>
        /// Validates that a user's phone number is not blacklisted.
        /// </summary>
        public class BlacklistPhoneValidator<TUser> : IUserValidator<TUser>
            where TUser : IdentityUser<Guid> // ✅ matches your inheritance
        {
            private static readonly HashSet<string> BlacklistedNumbers = new(StringComparer.OrdinalIgnoreCase)
        {
            "+10000000000",
            "+19999999999",
            "+18001234567"
        };

            public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
            {
                var phone = user?.PhoneNumber;

                if (string.IsNullOrWhiteSpace(phone))
                    return Task.FromResult(IdentityResult.Success);

                if (BlacklistedNumbers.Contains(phone))
                {
                    var error = new IdentityError
                    {
                        Code = "PhoneNumberBlacklisted",
                        Description = "Phone number is not allowed."
                    };

                    return Task.FromResult(IdentityResult.Failed(error));
                }

                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
