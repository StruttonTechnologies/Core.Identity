namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IRegisterRequest
    {
        string Email { get; }
        string Password { get; }
    }
}
