using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Fakes.Factories;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Fakes.Scenarios
{
    /// <summary>
    /// Fluent builder for composing identity login test scenarios.
    /// Wires up user, roles, and tokens for expressive, modular test setup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginScenario
    {
        public StubUser User { get; private set; } = default!;
        public IList<string> Roles { get; private set; } = [];
        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;

        private IJwtUserTokenManager<Guid>? _tokenManager;

        /// <summary>
        /// Starts a scenario with a default user.
        /// </summary>
        public static LoginScenario WithDefaultUser()
        {
            return new LoginScenario { User = TestAppUserIdentityFactory.CreateDefault() };
        }

        /// <summary>
        /// Starts a scenario with a user created by username.
        /// </summary>
        public static LoginScenario WithUser(string userName)
        {
            return new LoginScenario { User = TestAppUserIdentityFactory.Create(userName) };
        }

        /// <summary>
        /// Starts a scenario with a fully constructed user.
        /// </summary>
        public static LoginScenario WithUser(StubUser user)
        {
            return new LoginScenario { User = user };
        }

        /// <summary>
        /// Adds roles to the scenario.
        /// </summary>
        public LoginScenario WithRoles(params string[] roles)
        {
            Roles = roles.ToList();
            return this;
        }

        /// <summary>
        /// Injects a token manager to generate access and refresh tokens.
        /// </summary>
        public LoginScenario WithTokensFrom(IJwtUserTokenManager<Guid> tokenManager)
        {
            _tokenManager = tokenManager;
            return this;
        }

        /// <summary>
        /// Finalizes the scenario and generates tokens if a token manager is provided.
        /// </summary>
        public LoginScenario Build()
        {
            if (_tokenManager != null)
            {
                string userName = User.UserName ?? "defaultUser";
                string email = User.Email ?? $"{userName}@example.com";

                AccessToken = _tokenManager.GenerateAccessTokenAsync(
                    User.Id,
                    userName,
                    email,
                    Roles,
                    CancellationToken.None).Result;

                RefreshToken = _tokenManager.GenerateRefreshTokenAsync(
                    User.Id,
                    userName,
                    CancellationToken.None).Result;
            }

            return this;
        }
    }
}
