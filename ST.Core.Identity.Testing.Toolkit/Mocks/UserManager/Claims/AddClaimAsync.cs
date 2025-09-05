using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Security.Claims;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Claims
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> AddClaimAsync behavior.
    /// </summary>
    public static class AddClaimAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when adding a claim.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when adding a claim.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Add claim failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for AddClaimAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, Claim, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync((TestUser user, Claim claim) => resultFunc(user, claim));
            return mock;
        }
    }
}