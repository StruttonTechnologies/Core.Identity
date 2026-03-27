namespace StruttonTechnologies.Core.Identity.API.Contracts.Users
{
    public interface IUpdateUserRequest
    {
        public string UserId { get; }
        public string? Email { get; }
        public string? PhoneNumber { get; }
    }
}
