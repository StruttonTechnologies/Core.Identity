namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface ILinkExternalLoginRequest
    {
        string UserId { get; }
        string Provider { get; }
        string ProviderKey { get; }
        string Token { get; }
    }
}
