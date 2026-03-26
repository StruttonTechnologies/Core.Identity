namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePasswordRequest
    {
        string UserId { get; }
        string CurrentPassword { get; }
        string NewPassword { get; }
    }
}
