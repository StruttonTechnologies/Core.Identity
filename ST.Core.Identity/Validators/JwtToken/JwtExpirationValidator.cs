using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace ST.Core.Identity.Validators.JwtToken
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class JwtExpirationValidator : IValidator<JwtSecurityToken>
    {
        /// <summary>
        /// Validates that the JWT token has not expired.
        /// </summary>
        /// <param name="input">The JWT token to validate.</param>
        /// <returns>
        /// A <see cref="IValidationResult"/> indicating success if the token is valid,
        /// or failure if it has expired.
        /// </returns>
        public IValidationResult Validate(JwtSecurityToken input)
        {
            if (input.ValidTo < DateTime.UtcNow)
            {
                return ValidationResultFactory.Failure(
                    message: "JWT token has expired.",
                    code: "TokenExpired",
                    field: nameof(input.ValidTo));
            }

            return ValidationResultFactory.Success();
        }
    }
}
