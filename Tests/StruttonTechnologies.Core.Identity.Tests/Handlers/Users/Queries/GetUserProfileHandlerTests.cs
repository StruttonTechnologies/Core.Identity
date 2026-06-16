using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers;
using StruttonTechnologies.Core.Identity.Dtos.Users;
using StruttonTechnologies.Core.Identity.Stub.Entities;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Users.Queries;

[ExcludeFromCodeCoverage]
public class GetUserProfileHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenUserExists_ReturnsProfileDto()
    {
        GetUserProfileQuery request = new(TestUser.Id.ToString());
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
        UserManagerMock.Setup(x => x.GetUserIdAsync(TestUser)).ReturnsAsync(TestUser.Id.ToString());
        UserManagerMock.Setup(x => x.GetEmailAsync(TestUser)).ReturnsAsync(TestUser.Email);
        UserManagerMock.Setup(x => x.GetUserNameAsync(TestUser)).ReturnsAsync(TestUser.UserName);
        UserManagerMock.Setup(x => x.IsLockedOutAsync(TestUser)).ReturnsAsync(false);

        GetUserProfileHandler<StubUser> sut = new(UserManagerMock.Object);

        UserProfileResult result = await sut.Handle(request, CancellationToken.None);

        result.UserId.Should().Be(TestUser.Id.ToString());
        result.Email.Should().Be(TestUser.Email);
        result.IsLockedOut.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ThrowsArgumentNullException()
    {
        GetUserProfileQuery request = new(Guid.NewGuid().ToString());
        UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StubUser?)null);

        GetUserProfileHandler<StubUser> sut = new(UserManagerMock.Object);

        Func<Task> act = async () => await sut.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
