using StruttonTechnologies.Core.Identity.Models;
using StruttonTechnologies.Core.Identity.Validators.Composite;

namespace StruttonTechnologies.Core.Identity.Extensions
{
    /// <summary>
    /// Provides mapping helpers for authentication context models.
    /// </summary>
    public static class AuthenticationContextExtensions
    {
        /// <summary>
        /// Converts an <see cref="AuthenticationContext"/> instance to an <see cref="AuthContext"/> record.
        /// </summary>
        /// <param name="context">The source authentication context.</param>
        /// <returns>The mapped <see cref="AuthContext"/> instance.</returns>
        public static AuthContext ToAuthContext(this AuthenticationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            return new(context.ProviderName, context.SessionId, context.Status);
        }
    }
}
