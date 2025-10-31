using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Stub.Data;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating mocked <see cref="UserManager{TUser}"/> instances
    /// preconfigured with <see cref="StubUsers"/>.
    /// </summary>
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<StubUser>> Create(StubUser? user = null)
        {
            var store = new Mock<IUserStore<StubUser>>();
            var mgr = new Mock<UserManager<StubUser>>(
                store.Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var stub = user ?? StubUserData.Default;

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
            var userManager = Create(user);
            var signInManager = MockSignInManagerFactory.Create(userManager);

            signInManager.Setup(m => m.CreateUserPrincipalAsync(user)).ReturnsAsync(principal);

            return (userManager, signInManager);
        }
    }
}