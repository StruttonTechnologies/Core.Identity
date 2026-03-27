using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication
{
    [ExcludeFromCodeCoverage]
    public class AuthenticateUserHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenRequestIsValid_DelegatesToAuthenticationOrchestration()
        {
            AuthenticateUserCommand request = new("user@example.com", "password");
            AuthenticationResultDto expected = AuthenticationResultDto.SuccessResult("access-token");

            AuthenticationOrchestrationMock
                .Setup(x => x.AuthenticateAsync(request.Email, request.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            AuthenticateUserHandler<Guid> sut = new(AuthenticationOrchestrationMock.Object);

            AuthenticationResultDto result = await sut.Handle(request, CancellationToken.None);

            result.Should().Be(expected);
            AuthenticationOrchestrationMock.VerifyAll();
        }

        [Fact]
        public async Task Handle_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            AuthenticateUserHandler<Guid> sut = new(AuthenticationOrchestrationMock.Object);

            Func<Task> act = async () => await sut.Handle(null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
