using System.Security.Claims;

using Moq;

using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;

public static class RevocableTokenManagerFactory
{
    public static IRevocableTokenManager Create()
    {
        Mock<IRevocableTokenManager> mock = new Mock<IRevocableTokenManager>();

        mock.Setup(m => m.RevokeAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        mock.Setup(m => m.IsAccessTokenRevokedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        mock.Setup(m => m.ValidateTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string token) =>
            {
                ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "StubUser"),
                    new Claim(ClaimTypes.Email, "stub@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                }, "Mock");

                return new ClaimsPrincipal(identity);
            });

        mock.Setup(m => m.GetExpirationAsync(It.IsAny<string>()))
            .ReturnsAsync(DateTime.UtcNow.AddMinutes(15));

        return mock.Object;
    }
}
