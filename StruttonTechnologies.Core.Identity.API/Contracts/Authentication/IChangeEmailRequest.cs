namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangeEmailRequest
    {
        public string UserId { get; }
        public string NewEmail { get; }
        public string Token { get; }
    }
}
