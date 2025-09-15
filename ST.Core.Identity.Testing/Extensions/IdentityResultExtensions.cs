using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Testing.Toolkit.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IdentityResult"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IdentityResultExtensions
    {
        /// <summary>
        /// Converts the <see cref="IdentityResult"/> to a string representation of its errors.
        /// </summary>
        /// <param name="result">The <see cref="IdentityResult"/> to convert.</param>
        /// <returns>
        /// "Success" if the result succeeded; otherwise, a semicolon-separated list of error codes and descriptions.
        /// </returns>
        public static string ToErrorString(this IdentityResult result)
        {
            return result.Succeeded
                ? "Success"
                : string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
        }
    }
}