using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Abstractions.Orchistration.Login
{
    /// <summary>
    /// Represents a contract for identity user information used within the authentication orchestration layer.
    /// </summary>
    /// <remarks>
    /// This interface enables the application to work with strongly-typed <see cref="Guid"/> user identifiers,
    /// while remaining compatible with ASP.NET Core Identity, which typically expects string-based user IDs.
    /// Implementations should expose the user's unique identifier (<see cref="Id"/>), login name (<see cref="UserName"/>),
    /// and contact email (<see cref="Email"/>).
    /// </remarks>
    public interface IIdentityUserContract
    {
        /// <summary>
        /// Gets the unique identifier for the user as a <see cref="Guid"/>.
        /// <para>
        /// This enables the application to use strongly-typed GUIDs for user identification, while the underlying identity system may use string representations.
        /// </para>
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the username associated with the user.
        /// <para>
        /// This is typically used for login and display purposes.
        /// </para>
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets the email address associated with the user.
        /// <para>
        /// This is used for communication and as an alternative identifier.
        /// </para>
        /// </summary>
        string Email { get; }
    }
}
