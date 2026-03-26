using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users
{
    /// <summary>
    /// Defines user coordination operations used by the API layer.
    /// </summary>
    public interface IUserCoordinator
    {
        /// <summary>Gets the claims principal projection for the specified user.</summary>
        Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId);

        /// <summary>Gets the normalized email for the specified user.</summary>
        Task<string?> GetNormalizedEmailAsync(string userId);

        /// <summary>Gets the role names assigned to the specified user.</summary>
        Task<IReadOnlyList<string>> GetUserRolesAsync(string userId);

        /// <summary>Gets the user profile for the specified user.</summary>
        Task<UserProfileDto> GetUserProfileAsync(string userId);
    }
}
