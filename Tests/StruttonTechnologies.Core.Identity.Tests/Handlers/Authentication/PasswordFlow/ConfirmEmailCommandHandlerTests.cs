using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.PasswordFlow;

[ExcludeFromCodeCoverage]
public class ConfirmEmailCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_ConfirmsEmail()
    {
        ConfirmEmailCommand request = new(TestUser.Id.ToString(), "confirm-token");
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.ConfirmEmailAsync(TestUser, request.Token)).ReturnsAsync(IdentityResult.Success);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ConfirmEmailCommandHandler`2", UserManagerMock.Object);

        IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailedIdentityResult()
    {
        ConfirmEmailCommand request = new(Guid.NewGuid().ToString(), "confirm-token");
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((Stub.Entities.StubUser?)null);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ConfirmEmailCommandHandler`2", UserManagerMock.Object);

        IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

        result.Succeeded.Should().BeFalse();
    }
}
