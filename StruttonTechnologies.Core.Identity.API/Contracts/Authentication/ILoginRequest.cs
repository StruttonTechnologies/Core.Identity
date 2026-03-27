namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface ILoginRequest
    {
        public string Email { get; }
        public string Password { get; }
    }
}
