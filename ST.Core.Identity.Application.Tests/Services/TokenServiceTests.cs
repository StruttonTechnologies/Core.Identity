using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Builders;
using ST.Core.Identity.Mocks.Factories;
using ST.Core.Identity.Orchestration.UserManager;
using ST.Core.Identity.Stub.Data;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Test.Data;
using System.Security.Claims;

namespace ST.Core.Identity.Application.Tests.Services
{
    public class AuthenticationOrchestrationTests
    {
        private readonly StubUser _user = StubUserData.Default;
        private readonly ClaimsPrincipal _principal = TestClaimsPrincipalBuilder.CreateDefault();
        private readonly AuthenticationOrchestration<StubUser, Guid> _auth;

        public AuthenticationOrchestrationTests()
        {
            var userManager = MockUserManagerFactory.Create(_user);
            var signInManager = MockSignInManagerFactory.Create(userManager);
            signInManager.Setup(m => m.CreateUserPrincipalAsync(_user)).ReturnsAsync(_principal);
            signInManager.Setup(m => m.CheckPasswordSignInAsync(_user, It.IsAny<string>(), true))
                         .ReturnsAsync(SignInResult.Success);

            var tokenService = MockTokenServiceFactory.Create<Guid>();

            _auth = new AuthenticationOrchestration<StubUser, Guid>(
                userManager.Object,
                signInManager.Object,
                tokenService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsSuccessResult()
        {
            var result = await _auth.AuthenticateAsync(_user.Email, "password", CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(KnownTokens.ValidToken, result.Token);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsSuccessResult()
        {
            var result = await _auth.RegisterAsync("newuser@example.com", "securePassword123!", CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(KnownTokens.ValidToken, result.Token);
        }

        [Fact]
        public async Task SignOutAsync_RevokesAccessToken()
        {
            var tokenService = MockTokenServiceFactory.Create<Guid>();
            var orchestration = new AuthenticationOrchestration<StubUser, Guid>(
                MockUserManagerFactory.Create().Object,
                MockSignInManagerFactory.Create(MockUserManagerFactory.Create()).Object,
                tokenService.Object);

            await orchestration.SignOutAsync(KnownTokens.ValidToken, CancellationToken.None);

            tokenService.Verify(m => m.RevokeAccessTokenAsync(KnownTokens.ValidToken, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}