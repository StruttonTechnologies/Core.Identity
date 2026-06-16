using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.ExternalLogins;

[ExcludeFromCodeCoverage]
public class LinkExternalLoginCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_AddsLogin()
    {
        LinkExternalLoginCommand request = new(TestUser.Id.ToString(), "Google", "provider-key", "token");
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.AddLoginAsync(TestUser, It.Is<UserLoginInfo>(login => login.LoginProvider == request.Provider && login.ProviderKey == request.ProviderKey))).ReturnsAsync(IdentityResult.Success);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.LinkExternalLoginCommandHandler`2", UserManagerMock.Object);

        IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailedIdentityResult()
    {
        LinkExternalLoginCommand request = new(Guid.NewGuid().ToString(), "Google", "provider-key", "token");
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((Stub.Entities.StubUser?)null);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.LinkExternalLoginCommandHandler`2", UserManagerMock.Object);

        IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

        result.Succeeded.Should().BeFalse();
    }
}
