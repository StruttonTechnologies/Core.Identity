namespace StruttonTechnologies.Core.Identity.API.Contracts.Users
{
    public interface IUpdateUserRequest
    {
        string UserId { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
    }
}
