using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser>
        where TUser : IdentityUser, new() 
    {
        private readonly IUserValidator<TUser> _userValidator;

        /// <summary>
        /// Asynchronously updates the phone number for the specified user.
        /// Generates a change phone number token, applies the change, and logs any errors.
        /// </summary>
        /// <param name="user">The user entity whose phone number will be updated. Must not be null.</param>
        /// <param name="newPhoneNumber">The new phone number to set. Must not be null or whitespace.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// An <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// If the update fails, the result will contain error descriptions.
        /// </returns>
        public virtual async Task<IdentityResult> UpdatePhoneNumberAsync(TUser user, string newPhoneNumber, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(newPhoneNumber);

            user.PhoneNumber = newPhoneNumber;

            // Trigger validation via UpdateAsync (even if you discard the result)
            var validationResult = await _userManager.UpdateAsync(user);
            if (!validationResult.Succeeded)
            {
                _logger.LogWarning("Phone number validation failed for user {UserId}: {Errors}", user.Id,
                    string.Join(", ", validationResult.Errors.Select(e => e.Description)));

                return validationResult;
            }

            try
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber);
                var result = await _userManager.ChangePhoneNumberAsync(user, newPhoneNumber, token);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Phone number update failed for user {UserId}: {Errors}", user.Id, errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during phone number update for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Exception occurred: {ex.Message}"
                });
            }
        }
    }
}
