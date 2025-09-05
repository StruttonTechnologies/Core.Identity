using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> SetLockoutEnabledAsync behavior.
    /// </summary>
    public static class SetLockoutEnabledAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when setting lockout enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when setting lockout enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Set lockout enabled failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for SetLockoutEnabledAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, bool, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync((TestUser user, bool enabled) => resultFunc(user, enabled));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when setting lockout enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}