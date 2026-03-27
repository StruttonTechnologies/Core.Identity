using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.PasswordFlow
{
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExistsAndEmailConfirmed_ReturnsResetToken()
        {
            ForgotPasswordCommand request = new(TestUser.Email!);
            UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.IsEmailConfirmedAsync(TestUser)).ReturnsAsync(true);
            UserManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(TestUser)).ReturnsAsync("reset-token");

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2", UserManagerMock.Object);

            string result = await InternalHandlerFactory.InvokeHandleAsync<string>(sut, request);

            result.Should().Be("reset-token");
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsEmptyString()
        {
            ForgotPasswordCommand request = new("missing@example.com");
            UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((StruttonTechnologies.Core.Identity.Stub.Entities.StubUser?)null);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2", UserManagerMock.Object);

            string result = await InternalHandlerFactory.InvokeHandleAsync<string>(sut, request);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenEmailIsNotConfirmed_ReturnsEmptyString()
        {
            ForgotPasswordCommand request = new(TestUser.Email!);
            UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.IsEmailConfirmedAsync(TestUser)).ReturnsAsync(false);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2", UserManagerMock.Object);

            string result = await InternalHandlerFactory.InvokeHandleAsync<string>(sut, request);

            result.Should().BeEmpty();
        }
    }
}
