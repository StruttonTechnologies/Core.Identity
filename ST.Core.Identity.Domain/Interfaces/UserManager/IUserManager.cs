using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Domain.Interfaces.UserManager
{
    public interface IUserManager<TUser, TKey>
         where TUser : IdentityUser<TKey>, new()
         where TKey : IEquatable<TKey>
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
