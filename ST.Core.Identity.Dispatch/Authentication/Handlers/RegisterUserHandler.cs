using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Handlers.Authentication
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
            var user = new TUser();
            await _userManager.SetUserNameAsync(user, request.DisplayName);
            await _userManager.SetEmailAsync(user, request.Email);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                return RegistrationResultDto.Failure(error);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            return RegistrationResultDto.SuccessResult(userId);
        }
    }
}
