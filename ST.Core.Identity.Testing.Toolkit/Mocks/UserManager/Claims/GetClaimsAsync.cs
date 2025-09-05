using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Security.Claims;
using System.Collections.Generic;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Claims
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetClaimsAsync behavior.
    /// </summary>
    public static class GetClaimsAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific list of claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithClaims(IList<Claim> claims)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetClaimsAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(claims);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns an empty list of claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithEmpty()
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetClaimsAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(new List<Claim>());
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GetClaimsAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IList<Claim>> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetClaimsAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting claims.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetClaimsAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}