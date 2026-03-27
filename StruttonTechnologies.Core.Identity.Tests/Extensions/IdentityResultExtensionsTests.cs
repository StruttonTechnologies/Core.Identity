using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Extensions;

namespace StruttonTechnologies.Core.Identity.Tests.Extensions
{
    /// <summary>
    /// Contains test scenarios for <see cref="IdentityResultExtensions"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IdentityResultExtensionsTests
    {
        [Fact]
        public void ToErrorString_WithSuccessResult_ReturnsSuccess()
        {
            IdentityResult result = IdentityResult.Success;

            string message = result.ToErrorString();

            Assert.Equal("Success", message);
        }

        [Fact]
        public void ToErrorString_WithFailureResult_ReturnsJoinedErrors()
        {
            IdentityResult result = IdentityResult.Failed(
                new IdentityError { Code = "A", Description = "First" },
                new IdentityError { Code = "B", Description = "Second" });

            string message = result.ToErrorString();

            Assert.Contains("A: First", message, StringComparison.Ordinal);
            Assert.Contains("B: Second", message, StringComparison.Ordinal);
        }
    }
}
