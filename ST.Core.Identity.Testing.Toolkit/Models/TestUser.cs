using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Testing.Toolkit.Models
{
    /// <summary>
    /// Represents a test user for identity-related unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the roles assigned to the test user.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUser"/> class with default test values.
        /// </summary>
        public TestUser()
        {
            UserName = "test.user";
            Email = "test.user@example.com";
            EmailConfirmed = true;
            PasswordHash = "hashed-password";
        }
    }
}
