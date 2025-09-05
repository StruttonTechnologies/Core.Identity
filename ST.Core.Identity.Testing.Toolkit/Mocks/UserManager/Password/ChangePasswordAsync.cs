using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Password
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> ChangePasswordAsync behavior.
    /// </summary>
    public static class ChangePasswordAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when changing a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ChangePasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when changing a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Change password failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ChangePasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription! }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for ChangePasswordAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ChangePasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string oldPassword, string newPassword) => resultFunc(user, oldPassword, newPassword));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when changing a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.ChangePasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}