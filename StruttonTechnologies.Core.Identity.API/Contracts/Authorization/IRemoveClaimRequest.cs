namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IRemoveClaimRequest
    {
        public string UserId { get; }
        public string ClaimType { get; }
        public string ClaimValue { get; }
    }
}
