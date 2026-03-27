using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Mutations
{
    [ExcludeFromCodeCoverage]
    public class DisableUserCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenEnableLockoutSucceeds_SetsLockoutEndDate()
        {
            DisableUserCommand request = new(TestUser.Id.ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.SetLockoutEnabledAsync(TestUser, true)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.SetLockoutEndDateAsync(TestUser, DateTimeOffset.MaxValue)).ReturnsAsync(IdentityResult.Success);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.DisableUserCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeTrue();
        }
    }
}
