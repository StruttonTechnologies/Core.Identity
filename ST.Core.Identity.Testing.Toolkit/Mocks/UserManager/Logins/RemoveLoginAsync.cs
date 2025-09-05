using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Logins
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> RemoveLoginAsync behavior.
    /// </summary>
    public static class RemoveLoginAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when removing a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveLoginAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when removing a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Remove login failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveLoginAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for RemoveLoginAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveLoginAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string loginProvider, string providerKey) => resultFunc(user, loginProvider, providerKey));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when removing a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveLoginAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}