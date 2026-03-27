namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePasswordRequest
    {
        public string UserId { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }
    }
}
