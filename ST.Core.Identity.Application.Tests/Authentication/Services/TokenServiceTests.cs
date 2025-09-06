using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Application.Authentication.Services;
using Xunit;

namespace ST.Core.Identity.Application.Tests.Authentication.Services
{
    public class TokenServiceTests
    {
        private static JwtTokenOptions GetOptions()
        {
            var key = "supersecretkey1234567890supersecretkey";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return new JwtTokenOptions("issuer", "audience", key, creds, 60);
        }

        private static ClaimsPrincipal GetPrincipal()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "Admin")
        };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public void GenerateToken_ReturnsValidJwt()
        {
            // Arrange
            var options = Options.Create(GetOptions());
            var service = new TokenService(options);
            var principal = GetPrincipal();

            // Act
            var token = service.GenerateToken(principal);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "testuser");
        }

        [Fact]
        public void ValidateToken_ReturnsPrincipal_WhenTokenIsValid()
        {
            // Arrange
            var options = Options.Create(GetOptions());
            var service = new TokenService(options);
            var principal = GetPrincipal();
            var token = service.GenerateToken(principal);

            // Act
            var validated = service.ValidateToken(token);

            // Assert
            Assert.NotNull(validated);
            Assert.Equal("testuser", validated.Identity.Name);
        }

        [Fact]
        public void ValidateToken_ReturnsNull_WhenTokenIsInvalid()
        {
            // Arrange
            var options = Options.Create(GetOptions());
            var service = new TokenService(options);
            var invalidToken = "invalid.token.value";

            // Act
            var validated = service.ValidateToken(invalidToken);

            // Assert
            Assert.Null(validated);
        }

        [Fact]
        public void GetExpiration_ReturnsExpirationDate()
        {
            // Arrange
            var options = Options.Create(GetOptions());
            var service = new TokenService(options);
            var principal = GetPrincipal();
            var token = service.GenerateToken(principal);

            // Act
            var expiration = service.GetExpiration(token);

            // Assert
            Assert.NotNull(expiration);
            Assert.True(expiration > DateTime.UtcNow);
        }
    } 
}