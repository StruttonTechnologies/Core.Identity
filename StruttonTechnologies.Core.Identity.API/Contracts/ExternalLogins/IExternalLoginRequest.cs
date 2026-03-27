namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface IExternalLoginRequest
    {
        public string Provider { get; }
        public string IdToken { get; }
    }
}
