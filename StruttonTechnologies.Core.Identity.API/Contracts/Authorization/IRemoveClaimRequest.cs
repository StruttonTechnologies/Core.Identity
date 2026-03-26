namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IRemoveClaimRequest
    {
        string UserId { get; }
        string ClaimType { get; }
        string ClaimValue { get; }
    }
}
