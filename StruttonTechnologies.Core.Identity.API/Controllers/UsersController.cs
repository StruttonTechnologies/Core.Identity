using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using StruttonTechnologies.Core.API.Controllers.Responses;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserCoordinator _coordinator;

        public UsersController(
            ILogger<UsersController> logger,
            IUserCoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        /// <summary>
        /// Gets the normalized email for the specified user.
        /// </summary>
        [HttpGet("{id}/normalized-email")]
        public async Task<IActionResult> GetNormalizedEmail(string id)
        {
            string? result = await _coordinator.GetNormalizedEmailAsync(id);
            return ApiResponse.From(result);
        }

        /// <summary>
        /// Gets the roles assigned to the specified user.
        /// </summary>
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            IReadOnlyList<string> result = await _coordinator.GetUserRolesAsync(id);
            return ApiResponse.From(result, r => r.Count == 0, "User has no roles assigned.");
        }

        /// <summary>
        /// Gets the profile details for the specified user.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            UserProfileResult result = await _coordinator.GetUserProfileAsync(id);
            return ApiResponse.From(result, r => r is null, "User profile not found.");
        }
    }
}
