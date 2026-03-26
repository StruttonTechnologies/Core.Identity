namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IResetPasswordRequest
    {
        string UserId { get; }
        string Token { get; }
        string NewPassword { get; }
    }
}
