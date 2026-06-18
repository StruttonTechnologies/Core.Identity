using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
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

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ConfirmEmailCommandHandler`2",
            UserManagerMock.Object);

        ConfirmEmailResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ConfirmEmailResultDto>(sut, request);

        result.IsSuccess.Should().BeTrue();
        result.FailureReason.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        ConfirmEmailCommand request = new(Guid.NewGuid().ToString(), "confirm-token");

        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
            .ReturnsAsync((Stub.Entities.StubUser?)null);

        object sut = InternalHandlerFactory.Create(
            "StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ConfirmEmailCommandHandler`2",
            UserManagerMock.Object);

        ConfirmEmailResultDto result =
            await InternalHandlerFactory.InvokeHandleAsync<ConfirmEmailResultDto>(sut, request);

        result.IsSuccess.Should().BeFalse();
        result.FailureReason.Should().Contain(request.UserId);
    }
}
