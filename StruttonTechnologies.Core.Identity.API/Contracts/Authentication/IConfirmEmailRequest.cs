namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IConfirmEmailRequest
    {
        string UserId { get; }
        string Token { get; }
    }
}
