using MediatR;
using ST.Core.Identity.Dtos.Authentication;
using ST.Core.MediatR.Authentication.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto>
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(SignInManager<IdentityUser> signInManager, ITokenService tokenService)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);
        if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid login");

        var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
        return await _tokenService.GenerateTokenAsync(user);
    }
}
