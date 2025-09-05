using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager
{
    /// <summary>
    /// Factory for creating mock UserManager<TestUser> instances with optional overrides.
    /// Used to isolate identity logic in unit tests without relying on real stores or services.
    /// </summary>
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<TestUser>> Create(
            IPasswordHasher<TestUser>? hasher = null,
            IUserValidator<TestUser>[]? userValidators = null,
            IPasswordValidator<TestUser>[]? passwordValidators = null)
        {
            var store = new Mock<IUserStore<TestUser>>();

            return new Mock<UserManager<TestUser>>(
                store.Object,
                null, // IOptions<IdentityOptions>
                hasher ?? new Mock<IPasswordHasher<TestUser>>().Object,
                userValidators ?? Array.Empty<IUserValidator<TestUser>>(),
                passwordValidators ?? Array.Empty<IPasswordValidator<TestUser>>(),
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null  // ILogger<UserManager<TestUser>>
            );
        }
    }
}
