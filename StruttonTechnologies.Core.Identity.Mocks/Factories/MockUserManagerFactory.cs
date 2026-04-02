using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Stub.Data;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating mocked <see cref="UserManager{TUser}"/> instances
    /// preconfigured with <see cref="StubUser"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<StubUser>> Create(StubUser? user = null)
        {
            Mock<IUserStore<StubUser>> store = new Mock<IUserStore<StubUser>>();
            Mock<UserManager<StubUser>> mgr = new Mock<UserManager<StubUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            StubUser stub = user ?? StubUserData.Default;

            mgr.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(stub);

            mgr.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(stub);

            // For GetUserIdAsync/GetUserNameAsync return simple values or fallbacks so we don't access unresolved members.
            mgr.Setup(m => m.GetUserIdAsync(It.IsAny<StubUser>()))
                .ReturnsAsync("stub-user-id");

            mgr.Setup(m => m.GetUserNameAsync(It.IsAny<StubUser>()))
                .ReturnsAsync(stub.DisplayName ?? "stub-user");

            return mgr;
        }

        public static (Mock<UserManager<StubUser>>, Mock<SignInManager<StubUser>>) CreateWithPrincipal(
            StubUser user,
            ClaimsPrincipal principal)
        {
            Mock<UserManager<StubUser>> userManager = Create(user);
            Mock<SignInManager<StubUser>> signInManager = MockSignInManagerFactory.Create(userManager);

            signInManager.Setup(m => m.CreateUserPrincipalAsync(user)).ReturnsAsync(principal);

            return (userManager, signInManager);
        }
    }
}
