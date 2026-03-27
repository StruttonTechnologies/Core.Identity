namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IResetPasswordRequest
    {
        public string UserId { get; }
        public string Token { get; }
        public string NewPassword { get; }
    }
}
