using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Mocks.Factories;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.ExternalLogins;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class CoordinatorHandlerTestBase
    {
        protected CoordinatorHandlerTestBase()
        {
            TestUser = new StubUser
            {
                Id = Guid.NewGuid(),
                UserName = "stub.user",
                Email = "stub.user@example.com",
                DisplayName = "Stub User",
                IsEmailConfirmed = true,
                IsLockedOut = false,
            };

            UserManagerMock = MockUserManagerFactory.Create(TestUser);
            TokenOrchestrationMock = new Mock<ITokenOrchestration<Guid>>(MockBehavior.Strict);
            AuthenticationOrchestrationMock = new Mock<IAuthenticationOrchestration<Guid>>(MockBehavior.Strict);
            ExternalLoginIdentityValidatorMock = new Mock<IExternalLoginIdentityValidator>(MockBehavior.Strict);
            JwtUserTokenManagerMock = new Mock<IJwtUserTokenManager<Guid>>(MockBehavior.Strict);
            RefreshTokenStoreMock = new Mock<IRefreshTokenStore<Guid>>(MockBehavior.Strict);
            AccessTokenRevocationStoreMock = new Mock<IAccessTokenRevocationStore<Guid>>(MockBehavior.Strict);
        }

        protected StubUser TestUser { get; }

        protected Mock<UserManager<StubUser>> UserManagerMock { get; }

        protected Mock<ITokenOrchestration<Guid>> TokenOrchestrationMock { get; }

        protected Mock<IAuthenticationOrchestration<Guid>> AuthenticationOrchestrationMock { get; }

        protected Mock<IExternalLoginIdentityValidator> ExternalLoginIdentityValidatorMock { get; }

        protected Mock<IJwtUserTokenManager<Guid>> JwtUserTokenManagerMock { get; }

        protected Mock<IRefreshTokenStore<Guid>> RefreshTokenStoreMock { get; }

        protected Mock<IAccessTokenRevocationStore<Guid>> AccessTokenRevocationStoreMock { get; }

        protected static IdentityResult Failed(string description)
        {
            return IdentityResult.Failed(new IdentityError { Description = description });
        }

        protected static RefreshToken<Guid> ActiveRefreshToken(Guid userId, string token = "refresh-token")
        {
            return new()
            {
                UserId = userId,
                Token = token,
                Username = "stub.user",
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                IsRevoked = false,
            };
        }
    }
}
