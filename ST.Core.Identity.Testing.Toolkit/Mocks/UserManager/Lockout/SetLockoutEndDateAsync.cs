using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> SetLockoutEndDateAsync behavior.
    /// </summary>
    public static class SetLockoutEndDateAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when setting lockout end date.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEndDateAsync(It.IsAny<TestUser>(), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when setting lockout end date.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Set lockout end date failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEndDateAsync(It.IsAny<TestUser>(), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for SetLockoutEndDateAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, DateTimeOffset?, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEndDateAsync(It.IsAny<TestUser>(), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync((TestUser user, DateTimeOffset? endDate) => resultFunc(user, endDate));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when setting lockout end date.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.SetLockoutEndDateAsync(It.IsAny<TestUser>(), It.IsAny<DateTimeOffset?>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}