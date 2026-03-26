using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.UserManager;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    public class MockAuthenticationFactory<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public Mock<UserManager<TUser>> UserManagerMock { get; }
        public Mock<SignInManager<TUser>> SignInManagerMock { get; }
        public Mock<ITokenOrchestration<TKey>> TokenOrchestrationMock { get; }

        public AuthenticationOrchestration<TUser, TKey> Orchestration { get; }

        public MockAuthenticationFactory()
        {
            // Setup mocks
            UserManagerMock = new Mock<UserManager<TUser>>(
                Mock.Of<IUserStore<TUser>>(),
                new object(), // IOptions<IdentityOptions>
                new object(), // IPasswordHasher<TUser>
                new List<IUserValidator<TUser>>(), // IEnumerable<IUserValidator<TUser>>
                new List<IPasswordValidator<TUser>>(), // IEnumerable<IPasswordValidator<TUser>>
                new object(), // ILookupNormalizer
                new object(), // IdentityErrorDescriber
                new object(), // IServiceProvider
                new object()  // ILogger<UserManager<TUser>>
            );


            SignInManagerMock = MockSignInManagerFactory.Create(UserManagerMock);


            TokenOrchestrationMock = new Mock<ITokenOrchestration<TKey>>();

            // Create orchestration instance
            Orchestration = new AuthenticationOrchestration<TUser, TKey>(
                UserManagerMock.Object,
                SignInManagerMock.Object,
                TokenOrchestrationMock.Object);
        }
    }
}
