namespace ST.Core.Identity.Dtos.ExternalLogins
{
    public record LinkExternalLoginDto(string UserId, string Provider, string ProviderKey, string Token);
    public record UnlinkExternalLoginDto(string UserId, string Provider, string ProviderKey);
    public record ExternalLoginInfoDto(string Provider, string ProviderKey, string DisplayName);
}