using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.JwtTokens
{
    [ExcludeFromCodeCoverage]
    public class RevokeTokenCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenRevokingRefreshTokenByToken_UsesRefreshRevocation()
        {
            RevokeTokenCommand request = new(token: "refresh-token", isRefreshToken: true);

            JwtUserTokenManagerMock
                .Setup(x => x.RevokeRefreshTokenAsync(request.Token!, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            object sut = InternalHandlerFactory
                .Create("StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers.RevokeTokenCommandHandler`2", UserManagerMock.Object, JwtUserTokenManagerMock.Object);

            Unit result = await InternalHandlerFactory.InvokeHandleAsync<Unit>(sut, request);

            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_WhenRevokingAllTokensForUser_RevokesAccessAndRefreshTokens()
        {
            RevokeTokenCommand request = new(userId: TestUser.Id.ToString());
            UserManagerMock
                .Setup(x => x.FindByIdAsync(request.UserId!))
                .ReturnsAsync(TestUser);

            JwtUserTokenManagerMock
                .Setup(x => x.RevokeRefreshTokenAsync(request.Token!, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            JwtUserTokenManagerMock
                .Setup(x => x.RevokeAccessTokensAsync(TestUser.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            object sut = InternalHandlerFactory
                .Create("StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers.RevokeTokenCommandHandler`2", UserManagerMock.Object, JwtUserTokenManagerMock.Object);

            Unit result = await InternalHandlerFactory
                .InvokeHandleAsync<Unit>(sut, request);

            result.Should().Be(Unit.Value);
        }
    }
}
