using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Stub.Entities;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Utilities;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Users.Queries;

[ExcludeFromCodeCoverage]
public class GetUserByEmailQueryHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsUserFromUserManager()
    {
        GetUserByEmailQuery<StubUser> request = new(TestUser.Email!);
        UserManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(TestUser);

        object sut = InternalHandlerFactory.Create("StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers.GetUserByEmailQueryHandler`2", UserManagerMock.Object);

        StubUser? result = await InternalHandlerFactory.InvokeHandleAsync<StubUser?>(sut, request);

        result.Should().BeSameAs(TestUser);
    }
}
