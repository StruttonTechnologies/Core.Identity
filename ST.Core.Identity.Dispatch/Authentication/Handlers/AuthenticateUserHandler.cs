using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Application.Contracts;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Handlers
{
    public class AuthenticateUserHandler<TUser> : IRequestHandler<AuthenticateUserCommand, AuthenticationResultDto>
    where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticateUserHandler(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResultDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return AuthenticationResultDto.Failure("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return AuthenticationResultDto.Failure("Invalid credentials");

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var token = _tokenService.GenerateToken(principal);

            return AuthenticationResultDto.SuccessResult(token);
        }
    }
}
