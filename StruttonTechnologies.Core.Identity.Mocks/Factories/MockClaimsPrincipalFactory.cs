// <copyright file="MockClaimsPrincipalFactory.cs" company="Strutton Technologies">
// Copyright (c) Strutton Technologies. All rights reserved.
// </copyright>

using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Test.Data;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked <see cref="IUserClaimsPrincipalFactory{IdentityUser}"/>.
    /// Produces <see cref="ClaimsPrincipal"/> objects based on KnownUsers and KnownRoles.
    /// </summary>
    public static class MockClaimsPrincipalFactory
    {
        /// <summary>
        /// Creates a mocked <see cref="IUserClaimsPrincipalFactory{IdentityUser}"/>.
        /// </summary>
        /// <returns>A configured mock factory.</returns>
        public static Mock<IUserClaimsPrincipalFactory<IdentityUser>> Create()
        {
            Mock<IUserClaimsPrincipalFactory<IdentityUser>> mock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            mock.Setup(f => f.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync((IdentityUser? user) =>
                {
                    Domain.Entities.IdentityUser<Guid> fallbackUser = KnownUsers.Default;

                    string userId = user?.Id ?? fallbackUser.Id.ToString();
                    string userName = user?.UserName ?? fallbackUser.UserName ?? string.Empty;
                    string email = user?.Email ?? fallbackUser.Email ?? "unknown@test.local";

                    List<Claim> claims =
                    [
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, KnownRoles.Member),
                        new Claim(KnownClaims.Email, email),
                        new Claim(KnownClaims.Role, KnownRoles.Member),
                    ];

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "mock");

                    return new ClaimsPrincipal(identity);
                });

            return mock;
        }
    }
}
