using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> VerifyUserTokenAsync behavior.
    /// </summary>
    public static class VerifyUserTokenAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns true for VerifyUserTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithTrue()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns false for VerifyUserTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFalse()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for VerifyUserTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, string, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string provider, string purpose, string token) => resultFunc(user, provider, purpose, token));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when verifying a user token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}