using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> VerifyTwoFactorTokenAsync behavior.
    /// </summary>
    public static class VerifyTwoFactorTokenAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns true for VerifyTwoFactorTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithTrue()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyTwoFactorTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns false for VerifyTwoFactorTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFalse()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyTwoFactorTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for VerifyTwoFactorTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, bool> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyTwoFactorTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string provider, string token) => resultFunc(user, provider, token));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when verifying a two-factor token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.VerifyTwoFactorTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}