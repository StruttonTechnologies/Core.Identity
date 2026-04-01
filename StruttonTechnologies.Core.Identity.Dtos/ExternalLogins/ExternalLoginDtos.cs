using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.ExternalLogins
{
    [ExcludeFromCodeCoverage]
    public record LinkExternalLoginDto(string UserId, string Provider, string ProviderKey, string Token);

    [ExcludeFromCodeCoverage]
    public record UnlinkExternalLoginDto(string UserId, string Provider, string ProviderKey);

    [ExcludeFromCodeCoverage]
    public record ExternalLoginInfoDto(string Provider, string ProviderKey, string DisplayName);
}
