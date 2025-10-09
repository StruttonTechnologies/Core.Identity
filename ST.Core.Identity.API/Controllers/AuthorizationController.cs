using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.Core.API.Controllers.Responses;
using ST.Core.Identity.Dispatcher.Contracts.Authorization;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationDispatcher _dispatcher;
    private readonly ILogger<AuthorizationController> _logger;


    public AuthorizationController(
        ILogger<AuthorizationController> logger,
        IAuthorizationDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Gets a ClaimsPrincipal DTO for the specified user.
    /// </summary>
    [HttpGet("{userId}/claims")]
    public async Task<IActionResult> GetClaimsPrincipal(string userId)
    {
        var result = await _dispatcher.GetClaimsPrincipalAsync(userId);
        return ApiResponse.From(result, r => r.Claims.Count == 0, "User has no claims assigned.");
    }


    
}