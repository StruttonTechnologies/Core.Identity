using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Services.Authentication;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Scenarios;
using ST.Core.IdentityAccess.Fakes.JwtToken;
using ST.Core.IdentityAccess.Fakes.Stores;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Tests.Services
{
    public class InternalLoginWorkFlowTests
    {
        private readonly TestUserManager _userManager;
        private readonly TestTokenManager _tokenManager;
        private readonly ILogger<InternalLoginService<TestAppIdentityUser, Guid>> _logger;

        private readonly InternalLoginService<TestAppIdentityUser, Guid> _service;

        public InternalLoginWorkFlowTests()
        {
            _userManager = new TestUserManager(new InMemoryUserStore());
            _tokenManager = new TestTokenManager();
            _logger = MockLoggerFactory.Create<InternalLoginService<TestAppIdentityUser, Guid>>();

            _service = new InternalLoginService<TestAppIdentityUser, Guid>(
                _userManager,
                _tokenManager,
                _logger);
        }

        [Fact]
        public async Task LoginWorkflow_WithValidUser_ReturnsExpectedResponse()
        {
            var scenario = LoginScenario
                .WithUser("workflow.user")
                .WithRoles("Admin")
                .WithTokensFrom(_tokenManager)
                .Build();

            await _userManager.CreateAsync(scenario.User);
            await _userManager.AddToRolesAsync(scenario.User, scenario.Roles);
            await _userManager.AddPasswordAsync(scenario.User, "Secure123!");

            var request = new InternalLoginRequestDto(scenario.User.UserName, "Secure123!");

            var response = await _service.InternalLoginWorkFlow(request, CancellationToken.None);

            Assert.Equal(scenario.AccessToken, response.AccessToken);
            Assert.Equal(scenario.RefreshToken, response.RefreshToken);
            Assert.Equal(scenario.User.UserName, response.Username);
            Assert.False(response.RequiresTwoFactor);
        }
    }


}

