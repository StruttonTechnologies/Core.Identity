namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IAssignRoleRequest
    {
        public string UserId { get; }
        public string RoleName { get; }
    }
}
