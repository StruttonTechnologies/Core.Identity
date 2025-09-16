using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser>
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously retrieves all users.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TUser}"/> containing all user entities.
        /// Returns an empty collection if an exception occurs.
        /// </returns>
        public virtual async Task<IEnumerable<TUser>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // If you want async DB access, use: await _userManager.Users.ToListAsync(cancellationToken);
                return await Task.FromResult(_userManager.Users.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all users.");
                return Enumerable.Empty<TUser>();
            }
        }
    }
}
