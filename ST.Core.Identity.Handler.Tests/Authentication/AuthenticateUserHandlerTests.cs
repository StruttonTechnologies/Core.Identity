using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dispatch.Authentication.Handlers;
using ST.Core.Identity.Handler.Tests.Base;
using ST.Core.Identity.Mocks;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Orchestration.Contracts.UserManager;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Handler.Tests.Authentication
{
    public class AuthenticateUserHandlerTests : HandlerTestBase
    {
        protected readonly StubUser TestUser;
        protected readonly Mock<IAuthenticationOrchestration<Guid>> AuthOrchestrationMock;

        public AuthenticateUserHandlerTests()
        {
            TestUser = new StubUser
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "testuser@example.com",
                IsEmailConfirmed = true, // Ensure email is confirmed
                IsLockedOut = false      // Ensure user is not locked out
            };

            // initialize orchestration mock wrapper and expose the inner Mock for test setup
            //AuthOrchestrationMock = new MockAuthOrchestrationFactory().OrchestrationMock;

            UserManagerMock
                .Setup(m => m.FindByEmailAsync(TestUser.Email))
                .ReturnsAsync(TestUser);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsToken()
        {
            // Arrange
            SetupValidToken();

            SignInManagerMock
                .Setup(m => m.CheckPasswordSignInAsync(
                    TestUser,
                    "correct-password",
                    true))
                .ReturnsAsync(SignInResult.Success);

            SignInManagerMock
                .Setup(m => m.CreateUserPrincipalAsync(TestUser))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, TestUser.UserName)
                }, "mock")));

            var handler = CreateHandler();
            var command = new AuthenticateUserCommand(TestUser.Email, "correct-password");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Token.Should().Be("mock-token");
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            SignInManagerMock
                .Setup(m => m.PasswordSignInAsync(
                    TestUser.UserName,
                    "wrong-password",
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            var handler = CreateHandler();
            var command = new AuthenticateUserCommand(TestUser.Email, "wrong-password");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.FailureReason.Should().Contain("Invalid credentials");
        }

        private void SetupValidToken()
        {
            TokenServiceMock
                .Setup(t => t.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .Callback<ClaimsPrincipal, CancellationToken>((p, _) =>
                {
                    Console.WriteLine($"Received principal: {p.Identity?.Name}");
                })
                .ReturnsAsync("mock-token");
        }

        private AuthenticateUserHandler<Guid> CreateHandler() =>
            new AuthenticateUserHandler<Guid>(
                 AuthOrchestrationMock.Object
            );
    }
}