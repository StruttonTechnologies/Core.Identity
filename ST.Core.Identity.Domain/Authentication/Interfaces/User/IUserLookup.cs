using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides methods for looking up user information.
    /// </summary>
    public interface IUserLookupService<TUser> 
        where TUser : IdentityUser
    {
        Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken);
        Task<TUser?> FindByEmailAsync(string email, CancellationToken cancellationToken);
        Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<TUser?> FindByNameAsync(string username, CancellationToken cancellationToken);
        Task<IEnumerable<TUser>> GetAllUsersAsync(CancellationToken cancellationToken);
    }
}
