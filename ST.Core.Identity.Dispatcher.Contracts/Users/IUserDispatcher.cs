using ST.Core.Identity.Dtos.Authorization;
using ST.Core.Identity.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatcher.Contracts.Users
{
    public interface IUserDispatcher
    {
        Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId);
        Task<string?> GetNormalizedEmailAsync(string userId);
        Task<IReadOnlyList<string>> GetUserRolesAsync(string userId);
        Task<UserProfileDto> GetUserProfileAsync(string userId);
    }
}
