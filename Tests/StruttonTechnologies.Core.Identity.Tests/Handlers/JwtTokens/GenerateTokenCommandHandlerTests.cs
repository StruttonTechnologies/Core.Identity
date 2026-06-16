using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.JwtTokens;

[ExcludeFromCodeCoverage]
public class GenerateTokenCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_GeneratesTokenPair()
    {
        GenerateTokenCommand request = new(TestUser.Id.ToString());
        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15);
        DateTime refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.GetClaimsAsync(TestUser)).ReturnsAsync(new List<Claim> { new(ClaimTypes.Name, TestUser.UserName!) });
        UserManagerMock.Setup(x => x.GetRolesAsync(TestUser)).ReturnsAsync(new List<string> { "Admin" });
        UserManagerMock.Setup(x => x.GetUserNameAsync(TestUser)).ReturnsAsync(TestUser.UserName);
        UserManagerMock.Setup(x => x.GetEmailAsync(TestUser)).ReturnsAsync(TestUser.Email);
        JwtUserTokenManagerMock.Setup(x => x.GenerateAccessTokenAsync(TestUser.Id, TestUser.UserName!, TestUser.Email!, It.Is<IEnumerable<string>>(r => r.Single() == "Admin"), It.IsAny<CancellationToken>())).ReturnsAsync("access-token");
        JwtUserTokenManagerMock.Setup(x => x.GenerateRefreshTokenAsync(TestUser.Id, TestUser.UserName!, It.IsAny<CancellationToken>())).ReturnsAsync("refresh-token");
        JwtUserTokenManagerMock.Setup(x => x.GetExpirationAsync("access-token")).ReturnsAsync(accessTokenExpiresAtUtc);
        JwtUserTokenManagerMock.Setup(x => x.GetExpirationAsync("refresh-token")).ReturnsAsync(refreshTokenExpiresAtUtc);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers.GenerateTokenCommandHandler`2", UserManagerMock.Object, JwtUserTokenManagerMock.Object);

        TokenResponseDto result = await InternalHandlerFactory.InvokeHandleAsync<TokenResponseDto>(sut, request);

        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.AccessTokenExpiresAtUtc.Should().Be(accessTokenExpiresAtUtc);
        result.RefreshTokenExpiresAtUtc.Should().Be(refreshTokenExpiresAtUtc);
    }
}
