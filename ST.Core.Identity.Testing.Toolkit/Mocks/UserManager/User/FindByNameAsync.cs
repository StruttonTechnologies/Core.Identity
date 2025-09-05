using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> FindByNameAsync behavior.
    /// </summary>
    public static class FindByNameAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific user for FindByNameAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithUser(TestUser user)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns null for FindByNameAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithNull()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((TestUser?)null);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for FindByNameAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<string, TestUser?> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => resultFunc(name));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when finding a user by name.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}