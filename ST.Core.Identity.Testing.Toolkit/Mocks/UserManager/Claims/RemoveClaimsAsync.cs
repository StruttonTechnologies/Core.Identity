using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Claims
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> RemoveClaimsAsync behavior.
    /// </summary>
    public static class RemoveClaimsAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when removing claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimsAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Success);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when removing claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Remove claims failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimsAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for RemoveClaimsAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IEnumerable<Claim>, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimsAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync((TestUser user, IEnumerable<Claim> claims) => resultFunc(user, claims));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when removing claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.RemoveClaimsAsync(It.IsAny<TestUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}