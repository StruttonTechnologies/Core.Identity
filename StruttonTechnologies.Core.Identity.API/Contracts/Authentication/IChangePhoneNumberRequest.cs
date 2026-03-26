namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePhoneNumberRequest
    {
        string UserId { get; }
        string PhoneNumber { get; }
        string Token { get; }
    }
}
