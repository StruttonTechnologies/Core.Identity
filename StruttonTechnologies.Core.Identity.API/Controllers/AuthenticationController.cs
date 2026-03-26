using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using StruttonTechnologies.Core.API.Controllers.Responses;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.API.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationCoordinator _coordinator;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthenticationCoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);
            RegistrationResultDto result = await _coordinator.RegisterAsync(request.Email, request.Password, request.DisplayName);
            return ApiResponse.From(result, r => !r.Success, result.FailureReason ?? "Registration failed.");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);
            AuthenticationResultDto result = await _coordinator.AuthenticateAsync(request.Email, request.Password);
            return ApiResponse.From(result, r => !r.IsSuccess, result.FailureReason ?? "Authentication failed.");
        }

        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] SignOutRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);
            SignOutResultDto result = await _coordinator.SignOutAsync(request.Token);
            return ApiResponse.From(result, r => !r.Success, result.Message ?? "Sign-out failed.");
        }
    }
}
