using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Password
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> AddPasswordAsync behavior.
    /// </summary>
    public static class AddPasswordAsyncMock
    {
        /// <summary>
        /// Creates a <see cref="Mock{UserManager{TestUser}}"/> that returns <see cref="IdentityResult.Success"/> for <see cref="UserManager{TestUser}.AddPasswordAsync(TestUser, string)"/>.
        /// </summary>
        /// <returns>
        /// A mock <see cref="UserManager{TestUser}"/> configured to succeed when adding a password.
        /// </returns>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Creates a <see cref="Mock{UserManager{TestUser}}"/> that returns a failed <see cref="IdentityResult"/> for <see cref="UserManager{TestUser}.AddPasswordAsync(TestUser, string)"/>.
        /// </summary>
        /// <param name="errorDescription">The error description to include in the failure result. Defaults to "Add password failed."</param>
        /// <returns>
        /// A mock <see cref="UserManager{TestUser}"/> configured to fail when adding a password.
        /// </returns>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Add password failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription! }));
            return mock;
        }

        /// <summary>
        /// Creates a <see cref="Mock{UserManager{TestUser}}"/> that returns a custom <see cref="IdentityResult"/> for <see cref="UserManager{TestUser}.AddPasswordAsync(TestUser, string)"/>.
        /// </summary>
        /// <param name="resultFunc">A function that takes a <see cref="TestUser"/> and password and returns an <see cref="IdentityResult"/>.</param>
        /// <returns>
        /// A mock <see cref="UserManager{TestUser}"/> configured to return a custom result when adding a password.
        /// </returns>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, string, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync((TestUser user, string password) => resultFunc(user, password));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when adding a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}