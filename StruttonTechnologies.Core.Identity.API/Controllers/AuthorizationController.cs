using Microsoft.AspNetCore.Mvc;

using StruttonTechnologies.Core.API.Controllers.Responses;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.API.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationCoordinator _coordinator;

        public AuthorizationController(
            IAuthorizationCoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        /// <summary>
        /// Gets a ClaimsPrincipal DTO for the specified user.
        /// </summary>
        [HttpGet("{userId}/claims")]
        public async Task<IActionResult> GetClaimsPrincipal(string userId)
        {
            ClaimsPrincipalDto? result = await _coordinator.GetClaimsPrincipalAsync(userId);
            return ApiResponse.From(result, r => r.Claims.Count == 0, "User has no claims assigned.");
        }
    }
}