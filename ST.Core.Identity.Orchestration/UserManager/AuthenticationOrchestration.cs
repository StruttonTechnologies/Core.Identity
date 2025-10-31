using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Entities.Base;
using ST.Core.Identity.Dtos.Authentication;
using ST.Core.Identity.Orchestration.Contracts.JwtToken;
using ST.Core.Identity.Orchestration.Contracts.UserManager;

namespace ST.Core.Identity.Orchestration.UserManager
{
    public class AuthenticationOrchestration<TUser, TKey> : IAuthenticationOrchestration<TKey>
        where TUser : IdentityUserBase<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly ITokenOrchestration<TKey> _tokenService;

        public AuthenticationOrchestration(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ITokenOrchestration<TKey> TokenOrchestration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _tokenService = TokenOrchestration ?? throw new ArgumentNullException(nameof(TokenOrchestration));
        }

        public async Task<AuthenticationResultDto> AuthenticateAsync(string email, string password, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return AuthenticationResultDto.Failure("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return AuthenticationResultDto.Failure("Invalid credentials");

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var token = await _tokenService.GenerateTokenAsync(principal, ct);

            return AuthenticationResultDto.SuccessResult(token);
        }

        public async Task<AuthenticationResultDto> RegisterAsync(string email, string password, CancellationToken ct)
        {
            var user = new TUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return AuthenticationResultDto.Failure(errors);
            }

            // Optionally sign in immediately
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var token = await _tokenService.GenerateTokenAsync(principal, ct);

            return AuthenticationResultDto.SuccessResult(token);
        }



        public async Task SignOutAsync(string accessToken, CancellationToken ct)
        {
            // Clear Identity cookies (if you’re also using them)
            await _signInManager.SignOutAsync();

            // Revoke the JWT so it can’t be reused
            await _tokenService.RevokeAccessTokenAsync(accessToken, ct);
        }
    }
}