using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> ConfirmEmailAsync behavior.
    /// </summary>
    public static class ConfirmEmailAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when confirming email.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ConfirmEmailAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when confirming email.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Confirm email failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ConfirmEmailAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for ConfirmEmailAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ConfirmEmailAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string token) => resultFunc(user, token));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when confirming email.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ConfirmEmailAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}