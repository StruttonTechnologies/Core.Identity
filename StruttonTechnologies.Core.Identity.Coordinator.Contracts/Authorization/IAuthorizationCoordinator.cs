using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization
{
    /// <summary>
    /// Defines the authorization coordination contract for claims-based identity retrieval.
    /// </summary>
    public interface IAuthorizationCoordinator
    {
        /// <summary>Gets the claims principal projection for the specified user.</summary>
        Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId);
    }
}
