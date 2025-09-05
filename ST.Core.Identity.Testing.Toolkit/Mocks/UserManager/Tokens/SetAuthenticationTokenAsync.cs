using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> SetAuthenticationTokenAsync behavior.
    /// </summary>
    public static class SetAuthenticationTokenAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when setting an authentication token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetAuthenticationTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when setting an authentication token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Set authentication token failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetAuthenticationTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for SetAuthenticationTokenAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetAuthenticationTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string loginProvider, string tokenName, string tokenValue) => resultFunc(user, loginProvider, tokenName, tokenValue));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when setting an authentication token.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetAuthenticationTokenAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}