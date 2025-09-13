using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Authentication.Interfaces.UserManager;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Abstractions.Orchistration.Login
{
    /// <summary>
    /// Defines a service for handling internal user login authentication.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    public interface IInternalLoginService<TUser>
        where TUser : class
    {
        /// <summary>
        /// Authenticates a user using the provided internal login request.
        /// </summary>
        /// <param name="request">The internal login request data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="LoginResponseDto"/> representing the result of the authentication attempt.</returns>
        Task<LoginResponseDto> AuthenticateAsync(InternalLoginRequestDto request, CancellationToken cancellationToken);
    }
}
