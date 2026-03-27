using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.ExternalLogins
{
    [ExcludeFromCodeCoverage]
    public class UnlinkExternalLoginCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_RemovesLogin()
        {
            UnlinkExternalLoginCommand request = new(TestUser.Id.ToString(), "Google", "provider-key");
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.RemoveLoginAsync(TestUser, request.Provider, request.ProviderKey)).ReturnsAsync(IdentityResult.Success);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.UnlinkExternalLoginCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsFailedIdentityResult()
        {
            UnlinkExternalLoginCommand request = new(Guid.NewGuid().ToString(), "Google", "provider-key");
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StruttonTechnologies.Core.Identity.Stub.Entities.StubUser?)null);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.UnlinkExternalLoginCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeFalse();
        }
    }
}
