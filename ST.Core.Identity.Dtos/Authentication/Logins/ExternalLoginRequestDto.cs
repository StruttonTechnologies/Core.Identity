using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    /// <summary>
    /// Represents a request for external login authentication.
    /// </summary>
    /// <param name="Provider">The name of the external authentication provider (e.g., Google, Facebook).</param>
    /// <param name="ProviderKey">The unique identifier provided by the external authentication provider.</param>
    /// <param name="DisplayName">The display name associated with the external login, if available.</param>
    [ExcludeFromCodeCoverage]
    public record ExternalLoginRequestDto(
        string Provider,
        string ProviderKey,
        string? DisplayName = null
    );
}
