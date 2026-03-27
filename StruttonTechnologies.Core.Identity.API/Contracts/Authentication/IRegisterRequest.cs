namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IRegisterRequest
    {
        public string Email { get; }
        public string Password { get; }
    }
}
