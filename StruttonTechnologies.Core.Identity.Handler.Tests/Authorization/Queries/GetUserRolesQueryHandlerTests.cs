using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authorization.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserRolesQueryHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ReturnsRoles()
        {
            GetUserRolesQuery request = new(TestUser.Id.ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.GetRolesAsync(TestUser)).ReturnsAsync(new List<string> { "Admin", "User" });

            GetUserRolesQueryHandler<StubUser> sut = new(UserManagerMock.Object);

            IList<string> result = await sut.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(["Admin", "User"]);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsEmptyArray()
        {
            GetUserRolesQuery request = new(Guid.NewGuid().ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StubUser?)null);

            GetUserRolesQueryHandler<StubUser> sut = new(UserManagerMock.Object);

            IList<string> result = await sut.Handle(request, CancellationToken.None);

            result.Should().BeEmpty();
        }
    }
}
