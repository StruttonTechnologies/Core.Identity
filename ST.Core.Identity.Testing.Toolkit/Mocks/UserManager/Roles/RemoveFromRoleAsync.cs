using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Roles
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> RemoveFromRoleAsync behavior.
    /// </summary>
    public static class RemoveFromRoleAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when removing from a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when removing from a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Remove from role failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for RemoveFromRoleAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string role) => resultFunc(user, role));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when removing from a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}