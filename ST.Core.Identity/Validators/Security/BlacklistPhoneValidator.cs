using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a user's phone number is not blacklisted.
    /// Implements IUserValidator to integrate with ASP.NET Core Identity.
    /// </summary>
    public class BlacklistPhoneValidator<TUser> : IUserValidator<TUser>
        where TUser : IdentityUser<Guid>
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