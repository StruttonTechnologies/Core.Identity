using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.SignOut;

[ExcludeFromCodeCoverage]
public class SignOutHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenRefreshTokenIsPresent_RevokesRefreshTokenAndReturnsSuccess()
    {
        SignOutCommand request = new(
            AccessToken: null,
            RefreshToken: "refresh-token");

        TokenOrchestrationMock
            .Setup(x => x.RevokeRefreshTokenAsync(request.RefreshToken!, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        SignOutHandler<Guid> sut = new(TokenOrchestrationMock.Object);

        SignOutResultDto result = await sut.Handle(request, CancellationToken.None);

        result.Success.Should().BeTrue();

        TokenOrchestrationMock.Verify(
            x => x.RevokeRefreshTokenAsync("refresh-token", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTokenIsWhitespace_ThrowsArgumentException()
    {
        SignOutHandler<Guid> sut = new(TokenOrchestrationMock.Object);

        Func<Task> act = async () => await sut.Handle(new SignOutCommand(null, " "), CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
