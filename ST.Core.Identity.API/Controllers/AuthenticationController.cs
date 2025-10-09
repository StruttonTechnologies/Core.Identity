using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.Core.API.Controllers.Responses;
using ST.Core.Identity.Dispatcher.Contracts.Authentication;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.API.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthenticationController: ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationDispatcher _dispatcher;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthenticationDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var result = await _dispatcher.RegisterAsync(request.Email, request.Password, request.DisplayName);
            return ApiResponse.From(result, r => !r.Success, r.FailureReason ?? "Registration failed.");
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto request)
        {
            var result = await _dispatcher.AuthenticateAsync(request.Email, request.Password);
            return ApiResponse.From(result, r => !r.Success, r.FailureReason ?? "Authentication failed.");
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] SignOutRequestDto request)
        {
            var result = await _dispatcher.SignOutAsync(request.Token);
            return ApiResponse.From(result, r => !r.Success, r.Message ?? "Sign-out failed.");
        }
    }
}
