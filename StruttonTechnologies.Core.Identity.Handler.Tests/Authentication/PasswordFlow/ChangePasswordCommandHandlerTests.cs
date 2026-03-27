using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.PasswordFlow
{
    [ExcludeFromCodeCoverage]
    public class ChangePasswordCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ChangesPassword()
        {
            ChangePasswordCommand request = new(TestUser.Id.ToString(), "old-pass", "new-pass");
            IdentityResult expected = IdentityResult.Success;

            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.ChangePasswordAsync(TestUser, request.CurrentPassword, request.NewPassword)).ReturnsAsync(expected);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ChangePasswordCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Should().BeSameAs(expected);
            UserManagerMock.VerifyAll();
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsFailedIdentityResult()
        {
            ChangePasswordCommand request = new(Guid.NewGuid().ToString(), "old-pass", "new-pass");
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StruttonTechnologies.Core.Identity.Stub.Entities.StubUser?)null);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ChangePasswordCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.Description.Contains(request.UserId, StringComparison.Ordinal));
        }
    }
}
