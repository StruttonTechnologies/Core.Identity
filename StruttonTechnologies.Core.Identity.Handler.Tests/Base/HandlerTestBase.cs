using StruttonTechnologies.Core.Identity.Mocks.Factories;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Stub.Builders;
using StruttonTechnologies.Core.Identity.Stub.Data;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Base
{
    [ExcludeFromCodeCoverage]
#pragma warning disable CA1515 // Consider making public types internal
    public abstract class HandlerTestBase
#pragma warning restore CA1515 // Consider making public types internal
    {
        protected StubUser TestUser { get; }

        protected ClaimsPrincipal TestPrincipal { get; }

        protected Mock<UserManager<StubUser>> UserManagerMock { get; }

        protected Mock<SignInManager<StubUser>> SignInManagerMock { get; }

        protected Mock<ITokenOrchestration<Guid>> TokenServiceMock { get; }

        protected HandlerTestBase()
        {
            // Stub user and principal
            TestUser = StubUserData.Default;
            TestPrincipal = StubClaimsPrincipalBuilder.For(TestUser);

            // UserManager mock with preconfigured behavior
            UserManagerMock = MockUserManagerFactory.Create(TestUser);

            // SignInManager mock with principal setup
            SignInManagerMock = MockSignInManagerFactory.Create(UserManagerMock);
            SignInManagerMock
                .Setup(m => m.CreateUserPrincipalAsync(TestUser))
                .ReturnsAsync(TestPrincipal);

            // Token service mock with explicit generic type
            TokenServiceMock = MockTokenServiceFactory.Create<Guid>();
        }
    }
}
