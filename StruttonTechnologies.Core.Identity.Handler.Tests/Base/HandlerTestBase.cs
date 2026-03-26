using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Mocks.Factories;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Stub.Builders;
using StruttonTechnologies.Core.Identity.Stub.Data;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Base
{
    public abstract class HandlerTestBase
    {
        protected readonly StubUser TestUser;
        protected readonly ClaimsPrincipal TestPrincipal;
        protected readonly Mock<UserManager<StubUser>> UserManagerMock;
        protected readonly Mock<SignInManager<StubUser>> SignInManagerMock;
        protected readonly Mock<ITokenOrchestration<Guid>> TokenServiceMock;

        protected HandlerTestBase()
        {
            // Stub user and principal
            TestUser = StubUserData.Default;
            TestPrincipal = StubClaimsPrincipalBuilder.For(TestUser);

            // UserManager mock with preconfigured behavior
            UserManagerMock = MockUserManagerFactory.Create(TestUser);

            // SignInManager mock with principal setup
            SignInManagerMock = MockSignInManagerFactory.Create(UserManagerMock);
            SignInManagerMock.Setup(m => m.CreateUserPrincipalAsync(TestUser)).ReturnsAsync(TestPrincipal);

            // Token service mock with explicit generic type
            TokenServiceMock = MockTokenServiceFactory.Create<Guid>();
        }
    }
}
