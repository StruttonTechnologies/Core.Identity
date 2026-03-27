using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authorization.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserClaimsQueryHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ReturnsClaims()
        {
            GetUserClaimsQuery request = new(TestUser.Id.ToString());
            List<Claim> claims = [new(ClaimTypes.Email, TestUser.Email!)];

            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.GetClaimsAsync(TestUser)).ReturnsAsync(claims);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers.GetUserClaimsQueryHandler`2", UserManagerMock.Object);

            IList<Claim> result = await InternalHandlerFactory.InvokeHandleAsync<IList<Claim>>(sut, request);

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsEmptyList()
        {
            GetUserClaimsQuery request = new(Guid.NewGuid().ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StruttonTechnologies.Core.Identity.Stub.Entities.StubUser?)null);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers.GetUserClaimsQueryHandler`2", UserManagerMock.Object);

            IList<Claim> result = await InternalHandlerFactory.InvokeHandleAsync<IList<Claim>>(sut, request);

            result.Should().BeEmpty();
        }
    }
}
