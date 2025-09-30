using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Interfaces;
using ST.Core.Identity.Dtos.Authentication.RegisterUser;
using ST.Core.Identity.Exceptions;
using ST.Core.Identity.Validators.Identity;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ST.Core.Identity.Application.Services.RegistrerUser
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class RegisterUserService<TUser,TPerson, TKey>
    where TUser : IdentityUser<TKey>, new()
    where TPerson : PersonBase<TPerson>, new()
    where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IPersonRepository<TPerson> _personRepository;
        protected readonly ILogger<RegisterUserService<TUser,TPerson, TKey>> _logger;

        public RegisterUserService(
            UserManager<TUser> userManager,
            IPersonRepository<TPerson> personRepository,
            ILogger<RegisterUserService<TUser,TPerson, TKey>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task RegisterUserWorkFlow(RegistrationRequestDto request)
        {
            await ValidateRequestAsync(request);

            var user = await CreateUserInstanceAsync(request);
            await CreateUserAsync(user, request.Password);
            await AssignRolesAsync(user, request.Roles);

            _logger.LogInformation("User registered successfully: {UserName}", user.UserName);
        }

       protected internal virtual async Task UserNameIsAvailable(string userName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);
            var user = await _userManager.FindByNameAsync(userName);

            UserException.ThrowIfNotAvailable(user, userName);
        }


        public async Task CreatePerson (RegistrationRequestDto request)
        {

            var user = await CreateUserInstanceAsync(request);
            await CreateUserAsync(user, request.Password);
            await AssignRolesAsync(user, request.Roles);
            _logger.LogInformation("User registered successfully: {UserName}", user.UserName);
        }

        public async Task DoesPersonExist(RegistrationRequestDto request)
        {
 
            var user = await CreateUserInstanceAsync(request);
            await CreateUserAsync(user, request.Password);
            await AssignRolesAsync(user, request.Roles);
            _logger.LogInformation("User registered successfully: {UserName}", user.UserName);
        }


        public async Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto request)
        {
            await ValidateRequestAsync(request);

            //var user = await CreateUserAsync(user, request.Password);
            //await AssignRolesAsync(user, request.Roles);
            //var response = await BuildResponseAsync(user);
            //_logger.LogInformation("User registered successfully: {UserName}", user.UserName);
            //return response;
            return null;
        }


        protected internal virtual Task ValidateRequestAsync(RegistrationRequestDto request)
        {
            AggregateValidationException.Aggregate(
                "Registration Request validation failed",
                () => ArgumentNullException.ThrowIfNull(request),
                () => ArgumentException.ThrowIfNullOrWhiteSpace(request.UserName, nameof(request.UserName)),
                () => ArgumentException.ThrowIfNullOrWhiteSpace(request.Email, nameof(request.Email)),
                () => ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstName, nameof(request.FirstName)),
                () => ArgumentException.ThrowIfNullOrWhiteSpace(request.LastName, nameof(request.LastName)),
                () => PasswordValidationException.ThrowIfInvalid(request.Password),
                () => UserValidationException.ThrowIfUserNameInvalid(request.UserName)
            );

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
            var response = new RegistrationResponseDto(
                UserName: user.UserName,
                Email: user.Email,
                IsNewUser: true);



            return Task.FromResult(response);
        }
    }
}
