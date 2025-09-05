using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Security.Claims;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Logins
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> AddLoginAsync behavior.
    /// </summary>
    public static class AddLoginAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when adding a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddLoginAsync(It.IsAny<TestUser>(), It.IsAny<UserLoginInfo>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when adding a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Add login failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddLoginAsync(It.IsAny<TestUser>(), It.IsAny<UserLoginInfo>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for AddLoginAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, UserLoginInfo, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddLoginAsync(It.IsAny<TestUser>(), It.IsAny<UserLoginInfo>()))
                .ReturnsAsync((TestUser user, UserLoginInfo loginInfo) => resultFunc(user, loginInfo));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when adding a login.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddLoginAsync(It.IsAny<TestUser>(), It.IsAny<UserLoginInfo>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}