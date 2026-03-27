namespace StruttonTechnologies.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePhoneNumberRequest
    {
        public string UserId { get; }
        public string PhoneNumber { get; }
        public string Token { get; }
    }
}
