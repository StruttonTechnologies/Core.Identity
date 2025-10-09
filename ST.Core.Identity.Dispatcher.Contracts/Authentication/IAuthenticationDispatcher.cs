using ST.Core.Identity.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatcher.Contracts.Authentication
{
    public interface IAuthenticationDispatcher
    {
        Task<RegistrationResultDto> RegisterAsync(string email, string password, string displayName);
        Task<AuthenticationResultDto> AuthenticateAsync(string email, string password);
        Task<SignOutResultDto> SignOutAsync(string token);
    }
}
