using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Test.Data;
using System.Security.Claims;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked IUserClaimsPrincipalFactory.
    /// Produces ClaimsPrincipal objects based on KnownUsers and KnownRoles.
    /// </summary>
    public static class MockClaimsPrincipalFactory
    {
        public static Mock<IUserClaimsPrincipalFactory<IdentityUser>> Create()
        {
            var mock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            mock.Setup(f => f.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync((IdentityUser user) =>
                {
                    var fallbackUser = KnownUsers.Default;

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user?.Id ?? fallbackUser.Id),
                        new(ClaimTypes.Name, user?.UserName ?? fallbackUser.UserName ?? string.Empty),
                        new(ClaimTypes.Email, user?.Email ?? fallbackUser.Email ?? string.Empty),
                        new(ClaimTypes.Role, KnownRoles.Member),
                        new(KnownClaims.Email, user?.Email ?? "unknown@test.local"),
                        new(KnownClaims.Role, KnownRoles.Member)
                    };

                    var identity = new ClaimsIdentity(claims, authenticationType: "mock");
                    return new ClaimsPrincipal(identity);
                });

            return mock;
        }
    }
}