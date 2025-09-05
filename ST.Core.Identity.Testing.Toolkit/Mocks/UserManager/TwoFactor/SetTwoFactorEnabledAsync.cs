using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> SetTwoFactorEnabledAsync behavior.
    /// </summary>
    public static class SetTwoFactorEnabledAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when setting two-factor enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetTwoFactorEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when setting two-factor enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Set two-factor enabled failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetTwoFactorEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for SetTwoFactorEnabledAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, bool, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetTwoFactorEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ReturnsAsync((TestUser user, bool enabled) => resultFunc(user, enabled));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when setting two-factor enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetTwoFactorEnabledAsync(It.IsAny<TestUser>(), It.IsAny<bool>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}