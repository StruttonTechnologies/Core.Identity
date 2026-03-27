using System.Security.Claims;

namespace StruttonTechnologies.Core.Identity.Orchestration.Contracts.ExternalLogins
{
    /// <summary>
    /// Represents the validated identity returned from an external identity provider.
    /// </summary>
    public sealed record ExternalLoginIdentity(
        string Provider,
        string ProviderKey,
        string Email,
        string? DisplayName,
        IEnumerable<Claim>? Claims = null);

    /// <summary>
    /// Validates a provider-issued external login token and returns the normalized identity.
    /// </summary>
    public interface IExternalLoginIdentityValidator
    {
        public Task<ExternalLoginIdentity?> ValidateAsync(
            string provider,
            string idToken,
            CancellationToken cancellationToken);
    }
}
