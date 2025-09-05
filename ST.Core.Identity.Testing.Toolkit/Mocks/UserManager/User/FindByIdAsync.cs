using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> FindByIdAsync behavior.
    /// </summary>
    public static class FindByIdAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific user for FindByIdAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithUser(TestUser user)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns null for FindByIdAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithNull()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((TestUser?)null);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for FindByIdAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<string, TestUser?> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => resultFunc(id));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when finding a user by ID.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}