namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface IUnlinkExternalLoginRequest
    {
        public string UserId { get; }
        public string Provider { get; }
        public string ProviderKey { get; }
    }
}
