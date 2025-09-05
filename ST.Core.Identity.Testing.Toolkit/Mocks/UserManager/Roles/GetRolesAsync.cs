using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Roles
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetRolesAsync behavior.
    /// </summary>
    public static class GetRolesAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific list of roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithRoles(IList<string> roles)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetRolesAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(roles);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns an empty list of roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithEmpty()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetRolesAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(new List<string>());
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GetRolesAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IList<string>> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetRolesAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting roles.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetRolesAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}