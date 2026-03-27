using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.ExternalLogins;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// Handles validated external login requests and issues local tokens for the matching user.
    /// </summary>
    /// /// <typeparam name="TUser">
    /// The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.
    /// </typeparam>
    public class ExternalLoginCommandHandler<TUser, TKey>
        : IRequestHandler<ExternalLoginCommand, TokenResponseDto>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IExternalLoginIdentityValidator _externalLoginIdentityValidator;
        private readonly ITokenOrchestration<TKey> _tokenOrchestration;

        public ExternalLoginCommandHandler(
            UserManager<TUser> userManager,
            IExternalLoginIdentityValidator externalLoginIdentityValidator,
            ITokenOrchestration<TKey> tokenOrchestration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _externalLoginIdentityValidator = externalLoginIdentityValidator ?? throw new ArgumentNullException(nameof(externalLoginIdentityValidator));
            _tokenOrchestration = tokenOrchestration ?? throw new ArgumentNullException(nameof(tokenOrchestration));
        }

        public async Task<TokenResponseDto> Handle(
            ExternalLoginCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            ExternalLoginIdentity? externalIdentity = await _externalLoginIdentityValidator.ValidateAsync(
                request.Provider,
                request.IdToken,
                cancellationToken);

            if (externalIdentity is null)
            {
                throw new InvalidOperationException("The external login token could not be validated.");
            }

            TUser? user = await _userManager.FindByLoginAsync(
                externalIdentity.Provider,
                externalIdentity.ProviderKey);

            if (user is null && !string.IsNullOrWhiteSpace(externalIdentity.Email))
            {
                user = await _userManager.FindByEmailAsync(externalIdentity.Email);
            }

            if (user is null)
            {
                user = new TUser();

                await _userManager.SetUserNameAsync(user, externalIdentity.DisplayName ?? externalIdentity.Email);
                await _userManager.SetEmailAsync(user, externalIdentity.Email);

                IdentityResult createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    string error = createResult.Errors.FirstOrDefault()?.Description ?? "External login user creation failed.";
                    throw new InvalidOperationException(error);
                }
            }

            IList<UserLoginInfo> existingLogins = await _userManager.GetLoginsAsync(user);
            bool hasLogin = existingLogins.Any(x =>
                string.Equals(x.LoginProvider, externalIdentity.Provider, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.ProviderKey, externalIdentity.ProviderKey, StringComparison.Ordinal));

            if (!hasLogin)
            {
                IdentityResult addLoginResult = await _userManager.AddLoginAsync(
                    user,
                    new UserLoginInfo(externalIdentity.Provider, externalIdentity.ProviderKey, externalIdentity.Provider));

                if (!addLoginResult.Succeeded)
                {
                    string error = addLoginResult.Errors.FirstOrDefault()?.Description ?? "External login link failed.";
                    throw new InvalidOperationException(error);
                }
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity("Identity.Application");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, await _userManager.GetUserIdAsync(user)));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, await _userManager.GetUserNameAsync(user) ?? externalIdentity.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, await _userManager.GetEmailAsync(user) ?? externalIdentity.Email));

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            IEnumerable<Claim> externalClaims = externalIdentity.Claims ?? Enumerable.Empty<Claim>();
            foreach (Claim claim in externalClaims.Where(c =>
                c.Type is not ClaimTypes.NameIdentifier and
                not ClaimTypes.Name and
                not ClaimTypes.Email))
            {
                claimsIdentity.AddClaim(claim);
            }

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            string accessToken = await _tokenOrchestration.GenerateTokenAsync(principal, cancellationToken);
            DateTime expiresAt = _tokenOrchestration.GetExpirationTime();

            return new TokenResponseDto(accessToken, string.Empty, expiresAt);
        }
    }
}
