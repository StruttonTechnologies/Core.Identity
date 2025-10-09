using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Test.Data;

namespace ST.Core.Identity.Mocks.Factories
{
    public static class MockSignInManagerFactory
    {
        public static Mock<SignInManager<IdentityUser>> Create(UserManager<IdentityUser> userManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            var mgr = new Mock<SignInManager<IdentityUser>>(
                userManager,
                contextAccessor.Object,
                claimsFactory.Object,
                null!, null!, null!, null!);

            // Example: simulate successful login for KnownUsers.Default
            var defaultUserName = KnownUsers.Default.UserName ?? string.Empty;
            mgr.Setup(m => m.PasswordSignInAsync(
                    defaultUserName,
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
               .ReturnsAsync(SignInResult.Success);

            // Example: simulate failed login for unknown users
            mgr.Setup(m => m.PasswordSignInAsync(
                    It.Is<string>(u => u != defaultUserName),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
               .ReturnsAsync(SignInResult.Failed);

            return mgr;
        }
    }
}
