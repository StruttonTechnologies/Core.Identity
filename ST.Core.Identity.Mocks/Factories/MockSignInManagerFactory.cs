using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ST.Core.Identity.Mocks.Factories
{
    public static class MockSignInManagerFactory
    {
        public static Mock<SignInManager<TUser>> Create<TUser>(Mock<UserManager<TUser>> userManagerMock)
            where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();

            return new Mock<SignInManager<TUser>>(
                userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null!, null!, null!, null!);
        }
    }
}