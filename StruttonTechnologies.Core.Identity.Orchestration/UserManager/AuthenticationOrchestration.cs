using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager;

namespace StruttonTechnologies.Core.Identity.Orchestration.UserManager;

/// <summary>
/// Coordinates user authentication operations.
/// </summary>
/// <typeparam name="TUser">The identity user type.</typeparam>
/// <typeparam name="TKey">The identity key type.</typeparam>
public class AuthenticationOrchestration<TUser, TKey> : IAuthenticationOrchestration<TKey>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;
    private readonly SignInManager<TUser> _signInManager;
    private readonly ITokenOrchestration<TKey> _tokenService;

    public AuthenticationOrchestration(
        UserManager<TUser> userManager,
        SignInManager<TUser> signInManager,
        ITokenOrchestration<TKey> tokenOrchestration)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _tokenService = tokenOrchestration ?? throw new ArgumentNullException(nameof(tokenOrchestration));
    }

    public async Task<AuthenticationResultDto> AuthenticateAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        TUser? user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return AuthenticationResultDto.Failure("Invalid credentials");
        }

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return AuthenticationResultDto.Failure("Invalid credentials");
        }

        return await CreateAuthenticationResultAsync(user, email, ct);
    }

    public async Task<AuthenticationResultDto> RegisterAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        TUser user = new()
        {
            UserName = email,
            Email = email,
        };

        IdentityResult result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            string errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return AuthenticationResultDto.Failure(errors);
        }

        return await CreateAuthenticationResultAsync(user, email, ct);
    }

    public async Task SignOutAsync(string accessToken, CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        await _tokenService.RevokeAccessTokenAsync(accessToken, ct);
    }

    private async Task<AuthenticationResultDto> CreateAuthenticationResultAsync(
        TUser user,
        string fallbackUserName,
        CancellationToken ct)
    {
        ClaimsPrincipal principal = await _signInManager.CreateUserPrincipalAsync(user);

        string accessToken = await _tokenService.GenerateTokenAsync(principal, ct);
        string refreshToken = await _tokenService.GenerateRefreshTokenAsync(
            user.Id,
            user.UserName ?? user.Email ?? fallbackUserName,
            ct);

        DateTime? accessTokenExpiresAtUtc = await _tokenService.GetExpirationAsync(accessToken, ct);

        return AuthenticationResultDto.SuccessResult(
            accessToken,
            refreshToken,
            accessTokenExpiresAtUtc);
    }
}
