using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetLockoutEnabledAsync behavior.
    /// </summary>
    public static class GetLockoutEnabledAsyncMock
    {
        public static Mock<UserManager<TestUser>> WithValue(bool enabled)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEnabledAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(enabled);
            return mock;
        }

        public static Mock<UserManager<TestUser>> WithEnabled() => WithValue(true);
        public static Mock<UserManager<TestUser>> WithDisabled() => WithValue(false);

        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEnabledAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting lockout enabled.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEnabledAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}