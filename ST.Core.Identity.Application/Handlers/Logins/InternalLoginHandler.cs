using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands;
using ST.Core.Identity.Application.Services.Authentication;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Handlers.Logins
{
    /// <summary>
    /// Handles internal login requests by orchestrating the full authentication workflow.
    /// This includes user lookup, lockout validation, password verification, role retrieval,
    /// token generation, and response construction.
    /// </summary>
    /// <typeparam name="TUser">The user entity type implementing Identity contracts.</typeparam>
    /// <typeparam name="TKey">The type of the user's primary key.</typeparam>
    public class InternalLoginHandler<TUser, TKey> : IRequestHandler<InternalLoginCommand, LoginResponseDto>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly IInternalLoginService<TUser, TKey> _loginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalLoginHandler{TUser, TKey}"/> class.
        /// </summary>
        /// <param name="loginService">The orchestrator responsible for executing login steps.</param>
        public InternalLoginHandler(IInternalLoginService<TUser, TKey> loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Executes the internal login workflow.
        /// This method coordinates each authentication step:
        /// validating the request, locating the user, checking lockout status,
        /// verifying credentials, retrieving roles, generating tokens,
        /// and constructing the final response DTO.
        /// </summary>
        /// <param name="command">The login command containing the request DTO.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="LoginResponseDto"/> representing the result of the login attempt.</returns>
        public async Task<LoginResponseDto> Handle(InternalLoginCommand command, CancellationToken cancellationToken)
        {
            return await _loginService.LoginAsync(command.Request, cancellationToken);
        }
    }
}