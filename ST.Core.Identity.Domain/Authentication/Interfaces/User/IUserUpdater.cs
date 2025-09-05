using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides methods to update and manage user information.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    public interface IUserUpdater<TUser>
        where TUser : class
    {
        Task<IdentityResult> UpdateEmailAsync(TUser user, string newEmail, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateUsernameAsync(TUser user, string newUsername, CancellationToken cancellationToken);
        Task<IdentityResult> UpdatePhoneNumberAsync(TUser user, string newPhoneNumber, CancellationToken cancellationToken);
    }
}