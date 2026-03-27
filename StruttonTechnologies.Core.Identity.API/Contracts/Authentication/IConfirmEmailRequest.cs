namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IConfirmEmailRequest
    {
        public string UserId { get; }
        public string Token { get; }
    }
}
