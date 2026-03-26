using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace StruttonTechnologies.Core.Identity.Validators.JwtToken
{
    public class JwtExpirationValidator : IValidator<JwtSecurityToken>
    {
        /// <summary>
        /// Validates that the JWT token has not expired.
        /// </summary>
        /// <param name="input">The JWT token to validate.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating success if the token is valid,
        /// or failure if it has expired.
        /// </returns>
        public ValidationResult Validate(JwtSecurityToken input)
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentException.ThrowIfNullOrEmpty(
                input.ValidTo.ToString(CultureInfo.InvariantCulture), nameof(input));
            if (input.ValidTo < DateTime.UtcNow)
            {
                return ValidationResult.Failure(
                    message: "JWT token has expired.",
                    code: "TokenExpired",
                    field: nameof(input.ValidTo));
            }

            return ValidationResult.Success();
        }
    }
}
