using StruttonTechnologies.Core.Identity.API.Controllers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.API.Tests.Controllers
{
    /// <summary>
    /// Contains test scenarios for <see cref="AuthorizationController"/>.
    /// </summary>
    public class AuthorizationControllerTests
    {
        [Fact]
        public async Task GetClaimsPrincipalCallsCoordinator()
        {
            Mock<IAuthorizationCoordinator> coordinator = new();
            coordinator.Setup(x => x.GetClaimsPrincipalAsync("user-1"))
                .ReturnsAsync(new ClaimsPrincipalDto("Test", true, "User", Array.Empty<ClaimDto>()));

            AuthorizationController sut = new(coordinator.Object);

            _ = await sut.GetClaimsPrincipal("user-1");

            coordinator.Verify(x => x.GetClaimsPrincipalAsync("user-1"), Times.Once);
        }
    }
}
