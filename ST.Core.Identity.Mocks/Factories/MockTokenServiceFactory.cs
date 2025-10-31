using Moq;
using ST.Core.Identity.Test.Data;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ST.Core.Identity.Orchestration.Contracts.JwtToken;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked ITokenOrchestration.
    /// Used for orchestration-level JWT scenarios.
    /// </summary>
    public static class MockTokenServiceFactory
    {
        public static Mock<ITokenOrchestration<TKey>> Create<TKey>()
            where TKey : IEquatable<TKey>
        {
            var mock = new Mock<ITokenOrchestration<TKey>>();

            // Always return a known valid token for any ClaimsPrincipal
            mock.Setup(s => s.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(KnownTokens.ValidToken);

            // Expiration time
            mock.Setup(s => s.GetExpirationTime())
                .Returns(DateTime.UtcNow.AddMinutes(15));

            // Validation rules based on KnownTokens
            mock.Setup(s => s.ValidateTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, "known-user"),
                    new Claim(ClaimTypes.Name, "KnownUser"),
                    new Claim(ClaimTypes.Email, "known@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                }, "Mock")));

            mock.Setup(s => s.ValidateTokenAsync(KnownTokens.ExpiredToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal?)null);

            mock.Setup(s => s.ValidateTokenAsync(KnownTokens.InvalidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal?)null);

            // Revocation logic
            mock.Setup(s => s.IsAccessTokenRevokedAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mock.Setup(s => s.IsRefreshTokenRevokedAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mock.Setup(s => s.RevokeAccessTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mock.Setup(s => s.RevokeRefreshTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mock.Setup(s => s.GetExpirationAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(DateTime.UtcNow.AddMinutes(15));

            return mock;
        }
    }
}