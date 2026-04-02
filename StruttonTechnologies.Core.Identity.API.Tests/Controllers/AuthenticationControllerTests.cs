using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

using StruttonTechnologies.Core.Identity.API.Controllers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.API.Tests.Controllers
{
    /// <summary>
    /// Contains test scenarios for <see cref="AuthenticationController"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationCoordinator> _coordinator = new();
        private readonly AuthenticationController _sut;

        public AuthenticationControllerTests()
        {
            _sut = new AuthenticationController(new Mock<ILogger<AuthenticationController>>().Object, _coordinator.Object);
        }

        [Fact]
        public async Task RegisterWithNullRequestThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Register(null!));
        }

        [Fact]
        public async Task AuthenticateWithNullRequestThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Authenticate(null!));
        }

        [Fact]
        public async Task SignOutWithNullRequestThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.SignOut(null!));
        }

        [Fact]
        public async Task RegisterWithValidRequestCallsCoordinator()
        {
            RegisterUserRequestDto request = new("user@example.com", "Password1!", "User");
            _coordinator.Setup(x => x.RegisterAsync(request.Email, request.Password, request.DisplayName))
                .ReturnsAsync(RegistrationResultDto.SuccessResult("user-1"));

            _ = await _sut.Register(request);

            _coordinator.Verify(x => x.RegisterAsync(request.Email, request.Password, request.DisplayName), Times.Once);
        }

        [Fact]
        public async Task RegisterWithValidRequestReturnsOkWhenSuccessful()
        {
            RegisterUserRequestDto request = new("user@example.com", "Password1!", "User");
            _coordinator.Setup(x => x.RegisterAsync(request.Email, request.Password, request.DisplayName))
                .ReturnsAsync(RegistrationResultDto.SuccessResult("user-1"));

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.Register(request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task RegisterWithFailureReturnsErrorResponse()
        {
            RegisterUserRequestDto request = new("user@example.com", "Password1!", "User");
            _coordinator.Setup(x => x.RegisterAsync(request.Email, request.Password, request.DisplayName))
                .ReturnsAsync(RegistrationResultDto.Failure("Email already exists."));

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.Register(request);

            Assert.NotNull(result);
            _coordinator.Verify(x => x.RegisterAsync(request.Email, request.Password, request.DisplayName), Times.Once);
        }

        [Fact]
        public async Task AuthenticateWithValidRequestCallsCoordinator()
        {
            AuthenticateRequestDto request = new("user@example.com", "Password1!");
            _coordinator.Setup(x => x.AuthenticateAsync(request.Email, request.Password))
                .ReturnsAsync(AuthenticationResultDto.SuccessResult("token-123"));

            _ = await _sut.Authenticate(request);

            _coordinator.Verify(x => x.AuthenticateAsync(request.Email, request.Password), Times.Once);
        }

        [Fact]
        public async Task AuthenticateWithValidRequestReturnsOkWhenSuccessful()
        {
            AuthenticateRequestDto request = new("user@example.com", "Password1!");
            _coordinator.Setup(x => x.AuthenticateAsync(request.Email, request.Password))
                .ReturnsAsync(AuthenticationResultDto.SuccessResult("token-123"));

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.Authenticate(request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AuthenticateWithFailureReturnsErrorResponse()
        {
            AuthenticateRequestDto request = new("user@example.com", "WrongPassword");
            _coordinator.Setup(x => x.AuthenticateAsync(request.Email, request.Password))
                .ReturnsAsync(AuthenticationResultDto.Failure("Invalid credentials."));

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.Authenticate(request);

            Assert.NotNull(result);
            _coordinator.Verify(x => x.AuthenticateAsync(request.Email, request.Password), Times.Once);
        }

        [Fact]
        public async Task SignOutWithValidRequestCallsCoordinator()
        {
            SignOutRequestDto request = new("token-123");
            _coordinator.Setup(x => x.SignOutAsync(request.Token))
                .ReturnsAsync(SignOutResultDto.SuccessResult());

            _ = await _sut.SignOut(request);

            _coordinator.Verify(x => x.SignOutAsync(request.Token), Times.Once);
        }

        [Fact]
        public async Task SignOutWithValidRequestReturnsOkWhenSuccessful()
        {
            SignOutRequestDto request = new("token-123");
            _coordinator.Setup(x => x.SignOutAsync(request.Token))
                .ReturnsAsync(SignOutResultDto.SuccessResult());

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.SignOut(request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task SignOutWithFailureReturnsErrorResponse()
        {
            SignOutRequestDto request = new("invalid-token");
            _coordinator.Setup(x => x.SignOutAsync(request.Token))
                .ReturnsAsync(SignOutResultDto.Failure("Token not found."));

            Microsoft.AspNetCore.Mvc.IActionResult result = await _sut.SignOut(request);

            Assert.NotNull(result);
            _coordinator.Verify(x => x.SignOutAsync(request.Token), Times.Once);
        }
    }
}
