using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Data;
using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates usernames against reserved/blacklisted values and checks for existing users.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    public class UserValidator<TUser>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public UserValidator(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Validates a username asynchronously.
        /// </summary>
        /// <param name="userName">The username to validate.</param>
        /// <exception cref="UserValidationException">Thrown if the username fails any validation rules.</exception>
        public async Task ValidateUsernameAsync(string userName)
        {
            List<string> errors = [];

            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add("Username cannot be empty.");
            }

            if (KnownReservedUsernames.All.Contains(userName))
            {
                errors.Add("This username is reserved and cannot be used.");
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                TUser? existingUser = await _userManager.FindByNameAsync(userName);
                if (existingUser != null)
                {
                    errors.Add("This username is already taken.");
                }
            }

            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }
        }
    }
}
