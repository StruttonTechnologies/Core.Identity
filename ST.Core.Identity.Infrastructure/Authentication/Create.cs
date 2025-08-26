using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    /// <summary>
    /// Provides base functionality for user services.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public abstract partial class UserServiceBase<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Creates a new user with the specified password.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The created user entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user creation fails due to validation or other errors.
        /// </exception>
        public virtual async Task<TUser> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }
            return user;
        }
    }
}