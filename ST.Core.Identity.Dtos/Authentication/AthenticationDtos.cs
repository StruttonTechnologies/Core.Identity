using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{

    public record RegisterDto(string Email, string Password, string? ConfirmPassword = null);

    public record ExternalLoginDto(string Provider, string IdToken);

    public record RefreshTokenDto(string RefreshToken);

    public record TokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}