using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetAccessFailedCountAsync behavior.
    /// </summary>
    public static class GetAccessFailedCountAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific access failed count.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCount(int count)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetAccessFailedCountAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(count);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GetAccessFailedCountAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, int> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetAccessFailedCountAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting access failed count.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetAccessFailedCountAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}