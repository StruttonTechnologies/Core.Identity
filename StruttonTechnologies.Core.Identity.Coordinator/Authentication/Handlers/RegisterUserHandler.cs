using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    public class RegisterUserHandler<TUser> : IRequestHandler<RegisterUserCommand, RegistrationResultDto>
    where TUser : class, new()
    {
        private readonly UserManager<TUser> _userManager;

        public RegisterUserHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegistrationResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            TUser user = new TUser();
            await _userManager.SetUserNameAsync(user, request.DisplayName);
            await _userManager.SetEmailAsync(user, request.Email);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                string error = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                return RegistrationResultDto.Failure(error);
            }

            string userId = await _userManager.GetUserIdAsync(user);
            return RegistrationResultDto.SuccessResult(userId);
        }
    }
}
