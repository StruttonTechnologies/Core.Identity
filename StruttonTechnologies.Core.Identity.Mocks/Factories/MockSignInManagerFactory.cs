using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Moq;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    public static class MockSignInManagerFactory
    {
        public static Mock<SignInManager<TUser>> Create<TUser>(Mock<UserManager<TUser>> userManagerMock)
            where TUser : class
        {
            Mock<IHttpContextAccessor> contextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUserClaimsPrincipalFactory<TUser>> claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();

            return new Mock<SignInManager<TUser>>(
                userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null!, null!, null!, null!);
        }
    }
}
