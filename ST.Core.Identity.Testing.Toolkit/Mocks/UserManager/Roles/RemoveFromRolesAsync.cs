using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Roles
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> RemoveFromRolesAsync behavior.
    /// </summary>
    public static class RemoveFromRolesAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when removing from roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when removing from roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Remove from roles failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for RemoveFromRolesAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IEnumerable<string>, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync((TestUser user, IEnumerable<string> roles) => resultFunc(user, roles));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when removing from roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}