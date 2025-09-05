using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Roles
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> AddToRoleAsync behavior.
    /// </summary>
    public static class AddToRoleAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when adding to a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when adding to a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Add to role failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for AddToRoleAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string role) => resultFunc(user, role));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when adding to a role.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRoleAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}