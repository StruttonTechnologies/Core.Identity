using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> IsInRoleAsync behavior.
    /// </summary>
    public static class IsInRoleAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns true for IsInRoleAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithTrue()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsInRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns false for IsInRoleAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFalse()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsInRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for IsInRoleAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsInRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string role) => resultFunc(user, role));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when checking if user is in a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsInRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}   