using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.Core.API.Controllers.Responses;
using ST.Core.Identity.Dispatcher.Contracts.Users;

namespace ST.Core.Identity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserDispatcher _dispatcher;

        public UsersController(
            ILogger<UsersController> logger,
            IUserDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Gets the normalized email for the specified user.
        /// </summary>
        [HttpGet("{id}/normalized-email")]
        public async Task<IActionResult> GetNormalizedEmail(string id)
        {
            var result = await _dispatcher.GetNormalizedEmailAsync(id);
            return ApiResponse.From(result);
        }

        /// <summary>
        /// Gets the roles assigned to the specified user.
        /// </summary>
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var result = await _dispatcher.GetUserRolesAsync(id);
            return ApiResponse.From(result, r => r.Count == 0, "User has no roles assigned.");
        }

        /// <summary>
        /// Gets the profile details for the specified user.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var result = await _dispatcher.GetUserProfileAsync(id);
            return ApiResponse.From(result, r => r is null, "User profile not found.");
        }
    }
}