using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Security.Claims;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Claims
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> RemoveClaimAsync behavior.
    /// </summary>
    public static class RemoveClaimAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when removing a claim.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when removing a claim.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Remove claim failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for RemoveClaimAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, Claim, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ReturnsAsync((TestUser user, Claim claim) => resultFunc(user, claim));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when removing a claim.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimAsync(It.IsAny<TestUser>(), It.IsAny<Claim>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}