using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.SignOut
{
    [ExcludeFromCodeCoverage]
    public class SignOutHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenTokenIsPresent_RevokesRefreshTokenAndReturnsSuccess()
        {
            SignOutCommand request = new("refresh-token");
            TokenOrchestrationMock
                .Setup(x => x.RevokeRefreshTokenAsync(request.Token, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            SignOutHandler<Guid> sut = new(TokenOrchestrationMock.Object);

            SignOutResultDto result = await sut.Handle(request, CancellationToken.None);

            result.Success.Should().BeTrue();
            TokenOrchestrationMock.VerifyAll();
        }

        [Fact]
        public async Task Handle_WhenTokenIsWhitespace_ThrowsArgumentException()
        {
            SignOutHandler<Guid> sut = new(TokenOrchestrationMock.Object);

            Func<Task> act = async () => await sut.Handle(new SignOutCommand(" "), CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
