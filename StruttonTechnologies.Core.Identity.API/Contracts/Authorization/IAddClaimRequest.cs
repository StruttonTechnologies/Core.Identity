namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IAddClaimRequest
    {
        string UserId { get; }
        string ClaimType { get; }
        string ClaimValue { get; }
    }
}
