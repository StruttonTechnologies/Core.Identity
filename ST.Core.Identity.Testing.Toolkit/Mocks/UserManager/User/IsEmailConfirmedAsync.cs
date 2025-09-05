using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> IsEmailConfirmedAsync behavior.
    /// </summary>
    public static class IsEmailConfirmedAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns the specified value for IsEmailConfirmedAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithValue(bool confirmed)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsEmailConfirmedAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(confirmed);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for IsEmailConfirmedAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsEmailConfirmedAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when checking if email is confirmed.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.IsEmailConfirmedAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}