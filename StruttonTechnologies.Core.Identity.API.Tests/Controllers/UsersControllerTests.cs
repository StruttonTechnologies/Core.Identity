using Microsoft.Extensions.Logging;

using StruttonTechnologies.Core.Identity.API.Controllers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.API.Tests.Controllers
{
    /// <summary>
    /// Contains test scenarios for <see cref="UsersController"/>.
    /// </summary>
    public class UsersControllerTests
    {
        private static readonly string[] Value = new[] { "Admin" };
        private readonly Mock<IUserCoordinator> _coordinator = new();
        private readonly UsersController _sut;

        public UsersControllerTests()
        {
            _sut = new UsersController(
                new Mock<ILogger<UsersController>>().Object,
                _coordinator.Object);
        }

        [Fact]
        public async Task GetNormalizedEmail_Should_CallCoordinator()
        {
            _coordinator
                .Setup(x => x.GetNormalizedEmailAsync("user-1"))
                .ReturnsAsync("USER@EXAMPLE.COM");

            _ = await _sut.GetNormalizedEmail("user-1");

            _coordinator.Verify(
                x => x.GetNormalizedEmailAsync("user-1"),
                Times.Once);
        }

        [Fact]
        public async Task GetUserRoles_Should_CallCoordinator()
        {
            _coordinator
                .Setup(x => x.GetUserRolesAsync("user-1"))
                .ReturnsAsync(Value);

            _ = await _sut.GetUserRoles("user-1");

            _coordinator.Verify(
                x => x.GetUserRolesAsync("user-1"),
                Times.Once);
        }

        [Fact]
        public async Task GetUserProfile_Should_CallCoordinator()
        {
            _coordinator
                .Setup(x => x.GetUserProfileAsync("user-1"))
                .ReturnsAsync(new UserProfileResult(
                    UserId: "user-1",
                    Email: "user@example.com",
                    DisplayName: "user",
                    IsLockedOut: false));

            _ = await _sut.GetUserProfile("user-1");

            _coordinator.Verify(
                x => x.GetUserProfileAsync("user-1"),
                Times.Once);
        }
    }
}
