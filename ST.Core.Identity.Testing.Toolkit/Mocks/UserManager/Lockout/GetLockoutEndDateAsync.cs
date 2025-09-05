using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetLockoutEndDateAsync behavior.
    /// </summary>
    public static class GetLockoutEndDateAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns the specified date for GetLockoutEndDateAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithDate(DateTimeOffset date)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEndDateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(date);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GetLockoutEndDateAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, DateTimeOffset?> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEndDateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting lockout end date.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetLockoutEndDateAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}