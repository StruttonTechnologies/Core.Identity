using Moq;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using System.Security.Claims;

public static class RevocableTokenManagerFactory
{
    public static IRevocableTokenManager Create()
    {
        var mock = new Mock<IRevocableTokenManager>();

        mock.Setup(m => m.RevokeAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        mock.Setup(m => m.IsAccessTokenRevokedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        mock.Setup(m => m.ValidateTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string token) =>
            {
                var identity = new ClaimsIdentity(new[]
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