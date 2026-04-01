using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    /// <summary>
    /// Query to retrieve all users in the system.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system.</typeparam>
    [ExcludeFromCodeCoverage]
    public sealed record GetAllUsersQuery<TUser> : IRequest<IEnumerable<TUser>>
        where TUser : class;
}
