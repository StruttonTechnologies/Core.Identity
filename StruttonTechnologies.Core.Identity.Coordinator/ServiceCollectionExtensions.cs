using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator
{
    /// <summary>
    /// Extension methods for configuring Core Identity Coordinator services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity Coordinator services to the service collection.
        /// Registers all non-generic MediatR handlers for identity commands and queries, plus coordinator services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityCoordinator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            services.AddScoped<IAuthenticationCoordinator, AuthenticationCoordinator>();
            services.AddScoped<IAuthorizationCoordinator, AuthorizationCoordinator>();
            services.AddScoped<IUserCoordinator, UserCoordinator>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Coordinator services with closed generic handler registrations for the configured user and key types.
        /// </summary>
        /// <typeparam name="TUser">The configured identity user type.</typeparam>
        /// <typeparam name="TKey">The configured identity key type.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityCoordinator<TUser, TKey>(this IServiceCollection services)
            where TUser : IdentityUser<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            services.AddCoreIdentityCoordinator();

            // Authentication handlers
            services.AddTransient<IRequestHandler<AuthenticateUserCommand, AuthenticationResultDto>, AuthenticateUserHandler<TKey>>();
            services.AddTransient<IRequestHandler<RegisterUserCommand, RegistrationResultDto>, RegisterUserHandler<TUser>>();
            services.AddTransient<IRequestHandler<SignOutCommand, SignOutResultDto>, SignOutHandler<TKey>>();
            services.AddTransient<IRequestHandler<ChangePasswordCommand, IdentityResult>, ChangePasswordCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<ConfirmEmailCommand, IdentityResult>, ConfirmEmailCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<ForgotPasswordCommand, string>, ForgotPasswordCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<ResetPasswordCommand, IdentityResult>, ResetPasswordCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<ExternalLoginCommand, TokenResponseDto>, ExternalLoginCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<LinkExternalLoginCommand, IdentityResult>, LinkExternalLoginCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<UnlinkExternalLoginCommand, IdentityResult>, UnlinkExternalLoginCommandHandler<TUser, TKey>>();

            // Authorization handlers
            services.AddTransient<IRequestHandler<AddClaimCommand, IdentityResult>, AddClaimCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<AssignRoleCommand, IdentityResult>, AssignRoleCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetClaimsPrincipalQuery, ClaimsPrincipalDto>, GetClaimsPrincipalHandler<TUser>>();
            services.AddTransient<IRequestHandler<GetUserClaimsQuery, IList<System.Security.Claims.Claim>>, GetUserClaimsQueryHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetUserRolesQuery, IList<string>>, GetUserRolesQueryHandler<TUser>>();
            services.AddTransient<IRequestHandler<RemoveRoleCommand, IdentityResult>, RemoveRoleCommandHandler<TUser, TKey>>();

            // JWT token handlers
            services.AddTransient<IRequestHandler<GenerateTokenCommand, TokenResponseDto>, GenerateTokenCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<RefreshTokenCommand, TokenResponseDto>, RefreshTokenCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<RevokeTokenCommand, Unit>, RevokeTokenCommandHandler<TUser, TKey>>();

            // User handlers
            services.AddTransient<IRequestHandler<CreateUserCommand, IdentityResult>, CreateUserCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<DisableUserCommand, IdentityResult>, DisableUserCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<EnableUserCommand, IdentityResult>, EnableUserCommandHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetAllUsersQuery<TUser>, IEnumerable<TUser>>, GetAllUsersQueryHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetNormalizedEmailQuery, string?>, GetNormalizedEmailHandler<TUser>>();
            services.AddTransient<IRequestHandler<GetUserByEmailQuery<TUser>, TUser?>, GetUserByEmailQueryHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetUserByIdQuery, UserDetailResult>, GetUserByIdQueryHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetUserLoginsQuery, IList<UserLoginInfo>>, GetUserLoginsQueryHandler<TUser, TKey>>();
            services.AddTransient<IRequestHandler<GetUserProfileQuery, UserProfileResult>, GetUserProfileHandler<TUser>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Coordinator services with the default string key type.
        /// </summary>
        /// <typeparam name="TUser">The configured identity user type.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityCoordinator<TUser>(this IServiceCollection services)
            where TUser : IdentityUser<string>, new()
        {
            return services.AddCoreIdentityCoordinator<TUser, string>();
        }
    }
}
