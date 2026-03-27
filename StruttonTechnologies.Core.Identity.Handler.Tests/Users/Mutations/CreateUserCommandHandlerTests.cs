using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Mutations
{
    [ExcludeFromCodeCoverage]
    public class CreateUserCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WithPassword_CreatesUserWithPassword()
        {
            CreateUserCommand request = new("new.user", "new.user@example.com", "Password123!");

            UserManagerMock.Setup(x => x.SetUserNameAsync(It.IsAny<StubUser>(), request.UserName)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.SetEmailAsync(It.IsAny<StubUser>(), request.Email!)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.CreateAsync(It.IsAny<StubUser>(), request.Password!)).ReturnsAsync(IdentityResult.Success);

            CreateUserCommandHandler<StubUser, Guid> sut = new(UserManagerMock.Object);

            IdentityResult result = await sut.Handle(request, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WithoutPassword_CreatesUserWithoutPassword()
        {
            CreateUserCommand request = new("new.user", "new.user@example.com", null);

            UserManagerMock.Setup(x => x.SetUserNameAsync(It.IsAny<StubUser>(), request.UserName)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.SetEmailAsync(It.IsAny<StubUser>(), request.Email!)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.CreateAsync(It.IsAny<StubUser>())).ReturnsAsync(IdentityResult.Success);

            CreateUserCommandHandler<StubUser, Guid> sut = new(UserManagerMock.Object);

            IdentityResult result = await sut.Handle(request, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }
    }
}
