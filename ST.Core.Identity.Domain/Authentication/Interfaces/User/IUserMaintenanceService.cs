using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides maintenance operations for user entities.
    /// </summary>
    /// <typeparam name="TUser">The type of the user entity, derived from <see cref="IdentityUser"/>.</typeparam>
    public interface IUserMaintenanceService<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Asynchronously deletes the specified user.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task <bool> DeleteAsync(TUser user, CancellationToken cancellationToken);
    }
}
