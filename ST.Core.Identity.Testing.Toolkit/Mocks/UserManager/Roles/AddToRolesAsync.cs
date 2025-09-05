using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Roles
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> AddToRolesAsync behavior.
    /// </summary>
    public static class AddToRolesAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when adding to roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when adding to roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Add to roles failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for AddToRolesAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IEnumerable<string>, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync((TestUser user, IEnumerable<string> roles) => resultFunc(user, roles));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when adding to roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.AddToRolesAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<string>>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}