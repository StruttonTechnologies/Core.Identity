using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Coordinators
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationCoordinatorTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthenticationCoordinator _sut;

        public AuthenticationCoordinatorTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new AuthenticationCoordinator(_mediatorMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_SendsCommand_ReturnsResult()
        {
            string email = "test@example.com";
            string password = "TestPassword123!";
            string displayName = "Test User";
            RegistrationResultDto expectedResult = new(
                Success: true,
                UserId: Guid.NewGuid().ToString()
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<RegisterUserCommand>(c => c.Email == email && c.Password == password && c.DisplayName == displayName),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            RegistrationResultDto result = await _sut.RegisterAsync(email, password, displayName);

            result.Should().NotBeNull();
            result.Should().BeSameAs(expectedResult);
            result.Success.Should().BeTrue();
            _mediatorMock.Verify(x => x.Send(
                It.Is<RegisterUserCommand>(c => c.Email == email && c.Password == password && c.DisplayName == displayName),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenRegistrationFails_ReturnsFailedResult()
        {
            string email = "test@example.com";
            string password = "weak";
            string displayName = "Test User";
            RegistrationResultDto expectedResult = new(
                Success: false,
                FailureReason: "Password too weak"
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<RegisterUserCommand>(c => c.Email == email && c.Password == password && c.DisplayName == displayName),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            RegistrationResultDto result = await _sut.RegisterAsync(email, password, displayName);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.FailureReason.Should().NotBeNullOrEmpty();
            _mediatorMock.Verify(x => x.Send(
                It.Is<RegisterUserCommand>(c => c.Email == email && c.Password == password && c.DisplayName == displayName),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_SendsCommand_ReturnsResult()
        {
            string email = "test@example.com";
            string password = "TestPassword123!";
            AuthenticationResultDto expectedResult = new(
                IsSuccess: true,
                Token: "jwt-token"
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            AuthenticationResultDto result = await _sut.AuthenticateAsync(email, password);

            result.Should().NotBeNull();
            result.Should().BeSameAs(expectedResult);
            result.IsSuccess.Should().BeTrue();
            result.Token.Should().Be("jwt-token");
            _mediatorMock.Verify(x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WhenAuthenticationFails_ReturnsFailedResult()
        {
            string email = "test@example.com";
            string password = "WrongPassword";
            AuthenticationResultDto expectedResult = new(
                IsSuccess: false,
                Token: string.Empty,
                FailureReason: "Invalid credentials"
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            AuthenticationResultDto result = await _sut.AuthenticateAsync(email, password);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            _mediatorMock.Verify(x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SignOutAsync_SendsCommand_ReturnsResult()
        {
            string token = "jwt-token";
            SignOutResultDto expectedResult = new(
                Success: true,
                Message: "Sign out successful"
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<SignOutCommand>(c => c.Token == token),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            SignOutResultDto result = await _sut.SignOutAsync(token);

            result.Should().NotBeNull();
            result.Should().BeSameAs(expectedResult);
            result.Success.Should().BeTrue();
            _mediatorMock.Verify(x => x.Send(
                It.Is<SignOutCommand>(c => c.Token == token),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SignOutAsync_WhenSignOutFails_ReturnsFailedResult()
        {
            string token = "invalid-token";
            SignOutResultDto expectedResult = new(
                Success: false,
                Message: "Token not found"
            );

            _mediatorMock.Setup(x => x.Send(
                It.Is<SignOutCommand>(c => c.Token == token),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            SignOutResultDto result = await _sut.SignOutAsync(token);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            _mediatorMock.Verify(x => x.Send(
                It.Is<SignOutCommand>(c => c.Token == token),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
