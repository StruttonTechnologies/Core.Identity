using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.PasswordFlow;

[ExcludeFromCodeCoverage]
public class ResetPasswordCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_ResetsPassword()
    {
        ResetPasswordCommand request = new(TestUser.Id.ToString(), "reset-token", "NewPassword123!");

        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.ResetPasswordAsync(TestUser, request.Token, request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ResetPasswordCommandHandler`2",
            UserManagerMock.Object);

        ResetPasswordResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ResetPasswordResultDto>(sut, request);

        result.IsSuccess.Should().BeTrue();
        result.FailureReason.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        ResetPasswordCommand request = new(Guid.NewGuid().ToString(), "reset-token", "NewPassword123!");

        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
            .ReturnsAsync((Stub.Entities.StubUser?)null);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ResetPasswordCommandHandler`2",
            UserManagerMock.Object);

        ResetPasswordResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ResetPasswordResultDto>(sut, request);

        result.IsSuccess.Should().BeFalse();
        result.FailureReason.Should().Contain(request.UserId);
    }
}
