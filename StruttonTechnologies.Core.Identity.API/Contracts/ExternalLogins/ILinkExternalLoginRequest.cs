namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface ILinkExternalLoginRequest
    {
        public string UserId { get; }
        public string Provider { get; }
        public string ProviderKey { get; }
        public string Token { get; }
    }
}
