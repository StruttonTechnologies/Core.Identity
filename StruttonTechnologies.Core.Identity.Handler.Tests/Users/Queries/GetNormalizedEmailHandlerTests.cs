using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Users.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetNormalizedEmailHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ReturnsEmail()
        {
            GetNormalizedEmailQuery request = new(TestUser.Id.ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.GetEmailAsync(TestUser)).ReturnsAsync(TestUser.Email);

            GetNormalizedEmailHandler<StubUser> sut = new(UserManagerMock.Object);

            string? result = await sut.Handle(request, CancellationToken.None);

            result.Should().Be(TestUser.Email);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNull()
        {
            GetNormalizedEmailQuery request = new(Guid.NewGuid().ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StubUser?)null);

            GetNormalizedEmailHandler<StubUser> sut = new(UserManagerMock.Object);

            string? result = await sut.Handle(request, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
