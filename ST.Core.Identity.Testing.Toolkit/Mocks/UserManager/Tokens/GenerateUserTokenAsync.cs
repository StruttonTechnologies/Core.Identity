using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GenerateUserTokenAsync behavior.
    /// </summary>
    public static class GenerateUserTokenAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithToken(string token)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GenerateUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(token);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GenerateUserTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, string> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GenerateUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string provider, string purpose) => resultFunc(user, provider, purpose));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when generating a user token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GenerateUserTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}