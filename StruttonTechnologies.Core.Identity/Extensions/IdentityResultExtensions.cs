using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Extensions
{
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
            ArgumentNullException.ThrowIfNull(result);

            return result.Succeeded
                ? "Success"
                : string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
        }
    }
}
