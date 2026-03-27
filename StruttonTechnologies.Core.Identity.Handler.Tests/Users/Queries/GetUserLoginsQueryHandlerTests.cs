using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserLoginsQueryHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ReturnsLogins()
        {
            GetUserLoginsQuery request = new(TestUser.Id.ToString());
            IList<UserLoginInfo> expected = [new UserLoginInfo("Google", "provider-key", "Google")];

            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.GetLoginsAsync(TestUser)).ReturnsAsync(expected);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.GetUserLoginsQueryHandler`2", UserManagerMock.Object);

            IList<UserLoginInfo> result = await InternalHandlerFactory.InvokeHandleAsync<IList<UserLoginInfo>>(sut, request);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsEmptyList()
        {
            GetUserLoginsQuery request = new(Guid.NewGuid().ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StruttonTechnologies.Core.Identity.Stub.Entities.StubUser?)null);

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.GetUserLoginsQueryHandler`2", UserManagerMock.Object);

            IList<UserLoginInfo> result = await InternalHandlerFactory.InvokeHandleAsync<IList<UserLoginInfo>>(sut, request);

            result.Should().BeEmpty();
        }
    }
}
