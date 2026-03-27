using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that processes forgot password requests and generates password reset tokens.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class ForgotPasswordCommandHandler<TUser, TKey>
        : IRequestHandler<ForgotPasswordCommand, string>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public ForgotPasswordCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<string> Handle(
            ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByEmailAsync(request.Email);

            // Return empty string if user not found or email not confirmed
            // This prevents user enumeration attacks
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return string.Empty;
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}
