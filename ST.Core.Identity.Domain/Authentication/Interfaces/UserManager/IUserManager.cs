using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    public interface IUserManager<TUser>
        where TUser : class
    {
        Task<IdentityResult> UpdateEmailAsync(TUser user, string newEmail, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateUsernameAsync(TUser user, string newUsername, CancellationToken cancellationToken);
        Task<IdentityResult> UpdatePhoneNumberAsync(TUser user, string newPhoneNumber, CancellationToken cancellationToken);
        /// <summary>
        /// Asynchronously deletes the specified user.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task<bool> DeleteAsync(TUser user, CancellationToken cancellationToken);
    }
}
