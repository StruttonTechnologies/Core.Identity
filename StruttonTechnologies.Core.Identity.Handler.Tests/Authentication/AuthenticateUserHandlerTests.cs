using System.Security.Claims;

using FluentAssertions;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication
{
    public class AuthenticateUserHandlerTests : HandlerTestBase
    {
        private readonly StubUser _testUser;
        private readonly Mock<IAuthenticationOrchestration<Guid>> _authOrchestrationMock;

        public AuthenticateUserHandlerTests()
        {
            _testUser = new StubUser
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "testuser@example.com",
                IsEmailConfirmed = true,
                IsLockedOut = false
            };

            UserManagerMock
                .Setup(m => m.FindByEmailAsync(_testUser.Email))
                .ReturnsAsync(_testUser);

            _authOrchestrationMock = new Mock<IAuthenticationOrchestration<Guid>>();
            _authOrchestrationMock
                .Setup(a => a.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, CancellationToken>(async (email, password, ct) =>
                {
                    StubUser? user = await UserManagerMock.Object.FindByEmailAsync(email);
                    if (user == null)
                    {
                        return AuthenticationResultDto.Failure("Invalid credentials");
                    }

                    SignInResult checkResult = await SignInManagerMock.Object.CheckPasswordSignInAsync(user, password, true);
                    if (checkResult == null || !checkResult.Succeeded)
                    {
                        return AuthenticationResultDto.Failure("Invalid credentials");
                    }

                    ClaimsPrincipal principal = await SignInManagerMock.Object.CreateUserPrincipalAsync(user);
                    string token = await TokenServiceMock.Object.GenerateTokenAsync(principal, ct);

                    return AuthenticationResultDto.SuccessResult(token);
                });
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsToken()
        {
            SetupPasswordCheck(_testUser, "correct-password", SignInResult.Success);
            SetupPrincipal(_testUser);
            SetupToken("mock-token");

            AuthenticateUserHandler<Guid> handler = CreateHandler();
            AuthenticateUserCommand command = new AuthenticateUserCommand(_testUser.Email, "correct-password");

            AuthenticationResultDto result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Token.Should().Be("mock-token");
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ReturnsFailure()
        {
            SetupPasswordCheck(_testUser, "wrong-password", SignInResult.Failed);

            AuthenticateUserHandler<Guid> handler = CreateHandler();
            AuthenticateUserCommand command = new AuthenticateUserCommand(_testUser.Email, "wrong-password");

            AuthenticationResultDto result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.FailureReason.Should().Contain("Invalid credentials");
        }

        private void SetupPasswordCheck(StubUser user, string password, SignInResult result)
        {
            SignInManagerMock
                .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
                .ReturnsAsync(result);
        }

        private void SetupPrincipal(StubUser user)
        {
            SignInManagerMock
                .Setup(m => m.CreateUserPrincipalAsync(user))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }, "mock")));
        }

        private void SetupToken(string token)
        {
            TokenServiceMock
                .Setup(t => t.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(token);
        }

        private AuthenticateUserHandler<Guid> CreateHandler()
        {
            return new AuthenticateUserHandler<Guid>(_authOrchestrationMock.Object);
        }
    }
}
