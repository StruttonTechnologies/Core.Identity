namespace StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins
{
    public interface IExternalLoginRequest
    {
        string Provider { get; }
        string IdToken { get; }
    }
}
