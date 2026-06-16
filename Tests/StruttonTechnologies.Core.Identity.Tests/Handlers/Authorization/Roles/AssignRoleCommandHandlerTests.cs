using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authorization.Roles;

[ExcludeFromCodeCoverage]
public class AssignRoleCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_AddsRole()
    {
        AssignRoleCommand request = new(TestUser.Id.ToString(), "Admin");
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.AddToRoleAsync(TestUser, request.RoleName)).ReturnsAsync(IdentityResult.Success);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers.AssignRoleCommandHandler`2", UserManagerMock.Object);

        IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

        result.Succeeded.Should().BeTrue();
    }
}
