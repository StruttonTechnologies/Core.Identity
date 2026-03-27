using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities;
using StruttonTechnologies.Core.Identity.Handler.Tests.Utilities.AsyncQuerying;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAllUsersQueryHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_ReturnsAllUsersFromQueryableStore()
        {
            List<StubUser> users = [TestUser, new StubUser { Id = Guid.NewGuid(), UserName = "second", Email = "second@example.com" }];
            UserManagerMock.SetupGet(x => x.Users).Returns(new TestAsyncEnumerable<StubUser>(users));

            object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.GetAllUsersQueryHandler`2", UserManagerMock.Object);
            GetAllUsersQuery<StubUser> request = new();

            IEnumerable<StubUser> result = await InternalHandlerFactory.InvokeHandleAsync<IEnumerable<StubUser>>(sut, request);

            result.Should().HaveCount(2);
        }
    }
}
