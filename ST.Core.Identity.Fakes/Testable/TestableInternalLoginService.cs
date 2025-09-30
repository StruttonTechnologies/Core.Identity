using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Services.Authentication;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Testable
{
    public class TestableInternalLoginService : InternalLoginService<TestAppIdentityUser, Guid>
    {
        public TestableInternalLoginService(
            UserManager<TestAppIdentityUser> userManager,
            IJwtUserTokenManager<Guid> tokenManager,
            ILogger<InternalLoginService<TestAppIdentityUser, Guid>> logger)
            : base(userManager, tokenManager, logger)
        {
        }

        public Task<TestAppIdentityUser> ExposeFindUserRecordAsync(string username, CancellationToken ct) =>
            FindUserRecordAsync(username, ct);

        public Task ExposeEnsureUserNotLockedOutAsync(TestAppIdentityUser user) =>
            EnsureUserNotLockedOutAsync(user);

        public Task ExposeEnsureValidPasswordAsync(TestAppIdentityUser user, string password) =>
            EnsureValidPasswordAsync(user, password);

        public Task<string> ExposeGenerateAccessTokenAsync(TestAppIdentityUser user, IList<string> roles, CancellationToken ct) =>
            GenerateAccessTokenAsync(user, roles, ct);

        public Task<string> ExposeGenerateRefreshTokenAsync(TestAppIdentityUser user, CancellationToken ct) =>
            GenerateRefreshTokenAsync(user, ct);

        public Task<LoginResponseDto> ExposeBuildLoginResponseAsync(TestAppIdentityUser user, string accessToken, string refreshToken, CancellationToken ct) =>
            BuildLoginResponseAsync(user, accessToken, refreshToken, ct);
    }


}
