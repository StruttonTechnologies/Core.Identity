using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.Validators.JwtToken;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.JwtToken
{
    [ExcludeFromCodeCoverage]
    public class JwtExpirationValidatorTests
    {
        private readonly JwtExpirationValidator _validator;

        public JwtExpirationValidatorTests()
        {
            _validator = new JwtExpirationValidator();
        }

        [Fact]
        public void Validate_ReturnsSuccess_WhenTokenIsNotExpired()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.UtcNow.AddHours(1));

            ValidationResult result = _validator.Validate(token);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenTokenIsExpired()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.UtcNow.AddHours(-1));

            ValidationResult result = _validator.Validate(token);

            Assert.False(result.IsValid);
            Assert.Equal("JWT token has expired.", result.Message);
            Assert.Equal("TokenExpired", result.Code);
            Assert.Equal(nameof(token.ValidTo), result.Field);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenTokenExpirationIsExactlyNow()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.UtcNow);

            ValidationResult result = _validator.Validate(token);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ThrowsArgumentNullException_WhenTokenIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _validator.Validate(null!));
        }

        [Fact]
        public void Validate_HandlesTokenValidToMaxValue()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.MaxValue);

            ValidationResult result = _validator.Validate(token);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ReturnsSuccess_WhenTokenExpiresInFarFuture()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.UtcNow.AddYears(10));

            ValidationResult result = _validator.Validate(token);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenTokenExpiredRecentlyByMilliseconds()
        {
            JwtSecurityToken token = CreateToken(validTo: DateTime.UtcNow.AddMilliseconds(-100));

            ValidationResult result = _validator.Validate(token);

            Assert.False(result.IsValid);
        }

        private static JwtSecurityToken CreateToken(DateTime validTo, DateTime? notBefore = null)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            });

            DateTime effectiveNotBefore;
            DateTime issuedAt;

            if (notBefore.HasValue)
            {
                effectiveNotBefore = notBefore.Value;
                issuedAt = notBefore.Value;
            }
            else if (validTo == DateTime.MaxValue)
            {
                effectiveNotBefore = DateTime.MaxValue.AddHours(-1);
                issuedAt = DateTime.MaxValue.AddHours(-1);
            }
            else
            {
                effectiveNotBefore = validTo.AddHours(-1);
                issuedAt = validTo.AddHours(-1);
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = validTo,
                NotBefore = effectiveNotBefore,
                IssuedAt = issuedAt,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ThisIsAVeryLongSecretKeyForTestingPurposesOnly123456789")),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }
    }
}
