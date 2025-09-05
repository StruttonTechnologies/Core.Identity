using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> IsLockedOutAsync behavior.
    /// </summary>
    public static class IsLockedOutAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns the specified value for IsLockedOutAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithValue(bool lockedOut)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsLockedOutAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(lockedOut);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for IsLockedOutAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsLockedOutAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when checking if user is locked out.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsLockedOutAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}