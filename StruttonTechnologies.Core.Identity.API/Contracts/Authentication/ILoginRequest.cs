namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface ILoginRequest
    {
        string Email { get; }
        string Password { get; }
    }
}
