using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked IUserTwoFactorTokenProvider.
    /// Used for Identity flows like password reset, email confirmation, 2FA.
    /// </summary>
    public static class MockTokenProviderFactory
    {
        public static Mock<IUserTwoFactorTokenProvider<IdentityUser>> Create()
        {
            var mock = new Mock<IUserTwoFactorTokenProvider<IdentityUser>>();

            // Always generate a predictable token for tests
            mock.Setup(p => p.GenerateAsync(
                    It.IsAny<string>(),
                    It.IsAny<UserManager<IdentityUser>>(),
                    It.IsAny<IdentityUser>()))
                .ReturnsAsync("test-token");

            // ValidateAsync returns true only for "test-token"
            mock.Setup(p => p.ValidateAsync(
                    It.IsAny<string>(),
                    "test-token",
                    It.IsAny<UserManager<IdentityUser>>(),
                    It.IsAny<IdentityUser>()))
                .ReturnsAsync(true);

            mock.Setup(p => p.ValidateAsync(
                    It.IsAny<string>(),
                    It.Is<string>(t => t != "test-token"),
                    It.IsAny<UserManager<IdentityUser>>(),
                    It.IsAny<IdentityUser>()))
                .ReturnsAsync(false);

            // Optional: indicate whether this provider can be used for a given purpose
            mock.Setup(p => p.CanGenerateTwoFactorTokenAsync(
                    It.IsAny<UserManager<IdentityUser>>(),
                    It.IsAny<IdentityUser>()))
                .ReturnsAsync(true);

            return mock;
        }
    }
}
