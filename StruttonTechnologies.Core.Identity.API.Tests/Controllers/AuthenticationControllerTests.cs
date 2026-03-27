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
    }
}
