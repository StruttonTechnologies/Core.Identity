using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Defines methods for registering a user with or without a password.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user entity.</typeparam>
    public interface IUserRegistration<TUser> 
        where TUser : IdentityUser
    {
        /// <summary>
        /// Creates a new user with the specified password.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user.</returns>
        Task<TUser> CreateAsync(TUser user, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new user without setting a password.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user.</returns>
        Task<TUser> CreateNoPasswordAsync(TUser user, CancellationToken cancellationToken);
    }
}
