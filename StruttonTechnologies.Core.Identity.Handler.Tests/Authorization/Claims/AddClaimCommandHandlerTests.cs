using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authorization.Claims
{
    [ExcludeFromCodeCoverage]
    public class AddClaimCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_AddsClaim()
        {
            AddClaimCommand request = new(TestUser.Id.ToString(), ClaimTypes.Email, TestUser.Email!);
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.AddClaimAsync(TestUser, It.Is<Claim>(c => c.Type == request.ClaimType && c.Value == request.ClaimValue))).ReturnsAsync(IdentityResult.Success);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers.AddClaimCommandHandler`2", UserManagerMock.Object);

            IdentityResult result = await InternalHandlerFactory.InvokeHandleAsync<IdentityResult>(sut, request);

            result.Succeeded.Should().BeTrue();
        }
    }
}
