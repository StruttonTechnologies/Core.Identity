using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.PasswordFlow;

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

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2",
            UserManagerMock.Object);

        ForgotPasswordResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ForgotPasswordResultDto>(sut, request);

        result.IsSuccess.Should().BeTrue();
        result.ResetToken.Should().Be("reset-token");
        result.FailureReason.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsSuccessfulEmptyResult()
    {
        ForgotPasswordCommand request = new("missing@example.com");

        UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((Stub.Entities.StubUser?)null);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2",
            UserManagerMock.Object);

        ForgotPasswordResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ForgotPasswordResultDto>(sut, request);

        result.IsSuccess.Should().BeTrue();
        result.ResetToken.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_WhenEmailIsNotConfirmed_ReturnsSuccessfulEmptyResult()
    {
        ForgotPasswordCommand request = new(TestUser.Email!);

        UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.IsEmailConfirmedAsync(TestUser)).ReturnsAsync(false);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ForgotPasswordCommandHandler`2",
            UserManagerMock.Object);

        ForgotPasswordResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ForgotPasswordResultDto>(sut, request);

        result.IsSuccess.Should().BeTrue();
        result.ResetToken.Should().BeNullOrEmpty();
    }
}
