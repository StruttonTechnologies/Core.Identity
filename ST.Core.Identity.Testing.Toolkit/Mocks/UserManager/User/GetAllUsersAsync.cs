using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;
using System.Linq;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> Users property for GetAllUsersAsync behavior.
    /// </summary>
    public static class GetAllUsersAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns the specified users for the Users property.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithUsers(IEnumerable<TestUser> users)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns an empty user set for the Users property.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithEmpty()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.Users).Returns(Enumerable.Empty<TestUser>().AsQueryable());
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when accessing the Users property.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.Users).Throws(exception);
            return mock;
        }
    }
}