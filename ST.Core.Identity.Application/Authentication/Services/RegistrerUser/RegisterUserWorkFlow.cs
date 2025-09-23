using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.RegistrerUser
{
    public class RegisterUserWorkFlow
    {
        public async Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                await ValidateRequestAsync(request);
                var user = await CreateUserInstanceAsync(request);
                await CreateUserAsync(user, request.Password);
                await AssignRolesAsync(user, request.Roles);

                return await BuildResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for user {Username}", request?.UserName);
                throw;
            }
        }
    }
}
