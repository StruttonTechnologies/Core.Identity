namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangeEmailRequest
    {
        string UserId { get; }
        string NewEmail { get; }
        string Token { get; }
    }
}
