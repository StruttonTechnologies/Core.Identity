namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IRemoveRoleRequest
    {
        public string UserId { get; }
        public string RoleName { get; }
    }
}
