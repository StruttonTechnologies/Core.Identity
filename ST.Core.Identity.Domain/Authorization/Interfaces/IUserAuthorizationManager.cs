using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authorization.Interfaces
{
    public interface IUserAuthorizationManager<TUser> 
        where TUser : class

    {
        Task<IdentityResult> AddClaimAsync(TUser user, Claim claim, CancellationToken cancellationToken);
        Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim, CancellationToken cancellationToken);
        Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken);
        Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);

        Task<IdentityResult> AddToRoleAsync(TUser user, string role, CancellationToken cancellationToken);
        Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<string> roles, CancellationToken cancellationToken);
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role, CancellationToken cancellationToken);
        Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<string> roles, CancellationToken cancellationToken);
        Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken);
        Task<IList<TUser>> GetUsersInRoleAsync(string role, CancellationToken cancellationToken);
        Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken cancellationToken);
    }
}
