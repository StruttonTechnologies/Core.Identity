using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    /// <summary>
    /// Query to retrieve a user by their email address.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system.</typeparam>
    [ExcludeFromCodeCoverage]
    public sealed record GetUserByEmailQuery<TUser>(string Email) : IRequest<TUser?>
        where TUser : class;
}
