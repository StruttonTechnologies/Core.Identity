using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Password
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> CheckPasswordAsync behavior.
    /// </summary>
    public static class CheckPasswordAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns true for CheckPasswordAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithTrue()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CheckPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns false for CheckPasswordAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFalse()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CheckPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for CheckPasswordAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CheckPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string password) => resultFunc(user, password));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when checking a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CheckPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}