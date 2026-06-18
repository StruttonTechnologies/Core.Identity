using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

using RefreshTokenCommand = StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands.RefreshTokenCommand;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.JwtTokens;

[ExcludeFromCodeCoverage]
public class RefreshTokenCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenRefreshTokenIsActive_RotatesTokens()
    {
        RefreshTokenCommand request = new("refresh-token");

        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(20);

        RefreshToken<Guid> storedToken = ActiveRefreshToken(
            TestUser.Id,
            request.RefreshToken);

        RefreshTokenStoreMock
            .Setup(x => x.GetAsync(
                request.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storedToken);

        JwtUserTokenManagerMock
            .Setup(x => x.IsRefreshTokenRevokedAsync(
                request.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        UserManagerMock
            .Setup(x => x.FindByIdAsync(TestUser.Id.ToString()))
            .ReturnsAsync(TestUser);

        UserManagerMock
            .Setup(x => x.GetRolesAsync(TestUser))
            .ReturnsAsync(["User"]);

        UserManagerMock
            .Setup(x => x.GetUserNameAsync(TestUser))
            .ReturnsAsync(TestUser.UserName);

        UserManagerMock
            .Setup(x => x.GetEmailAsync(TestUser))
            .ReturnsAsync(TestUser.Email);

        JwtUserTokenManagerMock
            .Setup(x => x.GenerateAccessTokenAsync(
                TestUser.Id,
                TestUser.UserName!,
                TestUser.Email!,
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("new-access-token");

        JwtUserTokenManagerMock
            .Setup(x => x.GenerateRefreshTokenAsync(
                TestUser.Id,
                TestUser.UserName!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("new-refresh-token");

        RefreshTokenStoreMock
            .Setup(x => x.RevokeAsync(
                request.RefreshToken,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        JwtUserTokenManagerMock
            .Setup(x => x.GetExpirationAsync("new-access-token"))
            .ReturnsAsync(accessTokenExpiresAtUtc);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers.RefreshTokenCommandHandler`2",
            UserManagerMock.Object,
            JwtUserTokenManagerMock.Object,
            RefreshTokenStoreMock.Object);

        RefreshTokenResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<RefreshTokenResultDto>(
                sut,
                request);

        result.AccessToken.Should().Be("new-access-token");
        result.RefreshToken.Should().Be("new-refresh-token");

        result.AccessTokenExpiresAtUtc.Should()
            .BeCloseTo(accessTokenExpiresAtUtc, TimeSpan.FromSeconds(1));

        RefreshTokenStoreMock.Verify(
            x => x.RevokeAsync(
                request.RefreshToken,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsMissing_ReturnsFailureResult()
    {
        RefreshTokenCommand request = new("missing-token");

        RefreshTokenStoreMock
            .Setup(x => x.GetAsync(
                request.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken<Guid>?)null);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers.RefreshTokenCommandHandler`2",
            UserManagerMock.Object,
            JwtUserTokenManagerMock.Object,
            RefreshTokenStoreMock.Object);

        RefreshTokenResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<RefreshTokenResultDto>(
                sut,
                request);

        result.AccessToken.Should().BeNullOrWhiteSpace();
        result.RefreshToken.Should().BeNullOrWhiteSpace();

        RefreshTokenStoreMock.Verify(
            x => x.RevokeAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
