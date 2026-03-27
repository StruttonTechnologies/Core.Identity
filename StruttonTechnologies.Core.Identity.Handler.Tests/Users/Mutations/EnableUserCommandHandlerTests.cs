using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Mutations
{
    [ExcludeFromCodeCoverage]
    public class EnableUserCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ClearsLockoutEndDate()
        {
            EnableUserCommand request = new(TestUser.Id.ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.SetLockoutEndDateAsync(TestUser, null)).ReturnsAsync(IdentityResult.Success);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.EnableUserCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeTrue();
        }
    }
}
