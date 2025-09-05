using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Collections.Generic;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> GetValidTwoFactorProvidersAsync behavior.
    /// </summary>
    public static class GetValidTwoFactorProvidersAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always returns a specific list of two-factor providers.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithProviders(IList<string> providers)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetValidTwoFactorProvidersAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(providers);
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for GetValidTwoFactorProvidersAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IList<string>> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetValidTwoFactorProvidersAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when getting valid two-factor providers.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.GetValidTwoFactorProvidersAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}