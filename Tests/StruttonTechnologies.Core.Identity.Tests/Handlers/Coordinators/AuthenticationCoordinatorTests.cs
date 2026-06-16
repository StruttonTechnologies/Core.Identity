using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Coordinators;

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
        _mediatorMock.Verify(
            x => x.Send(
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
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<RegisterUserCommand>(c => c.Email == email && c.Password == password && c.DisplayName == displayName),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_SendsCommand_ReturnsResult()
    {
        string email = "test@example.com";
        string password = "TestPassword123!";

        AuthenticationResultDto expectedResult = new(
            IsSuccess: true,
            AccessToken: "access-token",
            RefreshToken: "refresh-token");

        _mediatorMock.Setup(x => x.Send(
            It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        AuthenticationResultDto result = await _sut.AuthenticateAsync(email, password);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedResult);
        result.IsSuccess.Should().BeTrue();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenAuthenticationFails_ReturnsFailedResult()
    {
        string email = "test@example.com";
        string password = "WrongPassword";
        AuthenticationResultDto expectedResult =
        AuthenticationResultDto.Failure("Invalid credentials");

        _mediatorMock.Setup(x => x.Send(
                It.Is<AuthenticateUserCommand>(
                    c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        AuthenticationResultDto result = await _sut.AuthenticateAsync(email, password);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<AuthenticateUserCommand>(c => c.Email == email && c.Password == password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task SignOutAsync_SendsCommand_ReturnsResult()
    {
        string accessToken = "access-token";
        string refreshToken = "refresh-token";

        SignOutResultDto expectedResult = new(
            Success: true,
            Message: "Sign out successful");

        _mediatorMock.Setup(x => x.Send(
            It.Is<SignOutCommand>(
                c => c.AccessToken == accessToken && c.RefreshToken == refreshToken),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        SignOutResultDto result = await _sut.SignOutAsync(
            accessToken,
            refreshToken);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedResult);
        result.Success.Should().BeTrue();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<SignOutCommand>(
                    c => c.AccessToken == accessToken && c.RefreshToken == refreshToken),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SignOutAsync_WhenSignOutFails_ReturnsFailedResult()
    {
        string accessToken = "invalid-access-token";
        string refreshToken = "invalid-refresh-token";

        SignOutResultDto expectedResult = new(
            Success: false,
            Message: "Token not found");

        _mediatorMock.Setup(x => x.Send(
            It.Is<SignOutCommand>(
                c => c.AccessToken == accessToken && c.RefreshToken == refreshToken),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        SignOutResultDto result = await _sut.SignOutAsync(
            accessToken,
            refreshToken);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<SignOutCommand>(
                    c => c.AccessToken == accessToken && c.RefreshToken == refreshToken),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
