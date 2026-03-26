namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface IUnlinkExternalLoginRequest
    {
        string UserId { get; }
        string Provider { get; }
        string ProviderKey { get; }
    }
}
