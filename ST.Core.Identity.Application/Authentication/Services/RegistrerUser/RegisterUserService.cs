using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.RegistrerUser
{
    public class RegisterUserService<TUser, TKey>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        protected readonly ILogger<RegisterUserService<TUser, TKey>> _logger;

        public RegisterUserService(
            UserManager<TUser> userManager,
            ILogger<RegisterUserService<TUser, TKey>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

       

        // 🔍 Step 1: Validate input
        protected internal virtual Task ValidateRequestAsync(RegistrationRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.UserName, nameof(request.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Email, nameof(request.Email));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password, nameof(request.Password));
            return Task.CompletedTask;
        }

        // 🧱 Step 2: Create user instance
        protected internal virtual Task<TUser> CreateUserInstanceAsync(RegistrationRequestDto request)
        {
            var user = new TUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            return Task.FromResult(user);
        }

        // 🛠️ Step 3: Persist user
        protected internal virtual async Task CreateUserAsync(TUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new SecurityException($"User registration failed: {errors}");
            }
        }

        // 🎭 Step 4: Assign roles
        protected internal virtual async Task AssignRolesAsync(TUser user, IList<string>? roles)
        {
            if (roles?.Any() != true)
                return;

            var result = await _userManager.AddToRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new SecurityException($"Role assignment failed: {errors}");
            }
        }

        // 📦 Step 5: Build response DTO
        protected internal virtual Task<RegistrationResponseDto> BuildResponseAsync(TUser user)
        {
            var response = new RegistrationResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsNewUser = true
            };

            return Task.FromResult(response);
        }
    }
}
