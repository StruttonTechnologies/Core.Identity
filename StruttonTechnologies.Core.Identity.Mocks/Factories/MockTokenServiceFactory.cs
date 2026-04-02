using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Moq;

using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Test.Data;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked ITokenOrchestration.
    /// Used for orchestration-level JWT scenarios.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MockTokenServiceFactory
    {
        public static Mock<ITokenOrchestration<TKey>> Create<TKey>()
            where TKey : IEquatable<TKey>
        {
            Mock<ITokenOrchestration<TKey>> mock = new Mock<ITokenOrchestration<TKey>>();

            // Always return a known valid token for any ClaimsPrincipal
            mock.Setup(s => s.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(KnownTokens.ValidToken);

            // Return a dynamic expiration time when asked (evaluated at call time)
            mock.Setup(s => s.GetExpirationTime())
                .Returns(() => DateTime.UtcNow.AddMinutes(15));

            // Validation rules based on KnownTokens
            mock.Setup(s => s.ValidateTokenAsync(It.Is<string>(t => t == KnownTokens.ValidToken), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity(
                    new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "known-user"),
                    new Claim(ClaimTypes.Name, "KnownUser"),
                    new Claim(ClaimTypes.Email, "known@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                }, "Mock")));

            // Unknown / invalid / expired tokens return null principal
            mock.Setup(s => s.ValidateTokenAsync(It.Is<string>(t => t == KnownTokens.ExpiredToken || t == KnownTokens.InvalidToken), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal?)null);

            mock.Setup(s => s.ValidateTokenAsync(It.Is<string>(t => t != KnownTokens.ValidToken && t != KnownTokens.ExpiredToken && t != KnownTokens.InvalidToken), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal?)null);

            // Revocation logic for known valid token
            mock.Setup(s => s.IsAccessTokenRevokedAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mock.Setup(s => s.IsRefreshTokenRevokedAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mock.Setup(s => s.RevokeAccessTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mock.Setup(s => s.RevokeRefreshTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // GetExpirationAsync returns a dynamic value for the known token
            mock.Setup(s => s.GetExpirationAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => DateTime.UtcNow.AddMinutes(15));

            // Default GetExpirationAsync for unknown tokens returns null
            mock.Setup(s => s.GetExpirationAsync(It.Is<string>(t => t != KnownTokens.ValidToken), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DateTime?)null);

            return mock;
        }
    }
}
