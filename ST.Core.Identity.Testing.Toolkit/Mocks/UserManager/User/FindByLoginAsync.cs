using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> FindByLoginAsync behavior.
    /// </summary>
    public static class FindByLoginAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific user for FindByLoginAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithUser(TestUser user)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns null for FindByLoginAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithNull()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser?)null);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for FindByLoginAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<string, string, TestUser?> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string loginProvider, string providerKey) => resultFunc(loginProvider, providerKey));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when finding a user by login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}