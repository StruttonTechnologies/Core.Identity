using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Password
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GeneratePasswordResetTokenAsync behavior.
    /// </summary>
    public static class GeneratePasswordResetTokenAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithToken(string token)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(token);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GeneratePasswordResetTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when generating a password reset token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}