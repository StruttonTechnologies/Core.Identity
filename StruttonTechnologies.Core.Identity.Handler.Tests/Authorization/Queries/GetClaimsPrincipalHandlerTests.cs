using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Exceptions;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authorization.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetClaimsPrincipalHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenUserExists_ReturnsMappedClaimsPrincipalDto()
        {
            GetClaimsPrincipalQuery request = new(TestUser.Id.ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(TestUser);
            UserManagerMock.Setup(x => x.GetClaimsAsync(TestUser)).ReturnsAsync(new List<Claim>
            {
                new(ClaimTypes.Name, "Stub User"),
                new(ClaimTypes.Role, "Admin"),
            });

            GetClaimsPrincipalHandler<StubUser> sut = new(UserManagerMock.Object);

            ClaimsPrincipalDto result = await sut.Handle(request, CancellationToken.None);

            result.AuthenticationType.Should().Be("Identity.Application");
            result.Claims.Should().Contain(x => x.Type == ClaimTypes.Role && x.Value == "Admin");
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            GetClaimsPrincipalQuery request = new(Guid.NewGuid().ToString());
            UserManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((StubUser?)null);

            GetClaimsPrincipalHandler<StubUser> sut = new(UserManagerMock.Object);

            Func<Task> act = async () => await sut.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}
