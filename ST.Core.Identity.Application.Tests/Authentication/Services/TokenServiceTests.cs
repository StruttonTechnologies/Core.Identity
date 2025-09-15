using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Application.Authentication.Services.JwtTokens;
using ST.Core.Identity.Application.Authentication.Services.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            var service = new TokenService(GetOptions());
            var principal = GetPrincipal();

            // Act
            var token = service.GenerateToken(principal);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "testuser");
            Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        }

        [Fact]
        public void GetExpirationTime_ReturnsFutureTimestamp()
        {
            // Arrange
            var service = new TokenService(GetOptions());

            // Act
            var expiration = service.GetExpirationTime();

            // Assert
            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(60));
        }

        [Fact]
        public void RevokeToken_MarksTokenAsRevoked()
        {
            // Arrange
            var service = new TokenService(GetOptions());
            var principal = GetPrincipal();
            var token = service.GenerateToken(principal);

            // Act
            service.RevokeToken(token);

            // Assert
            Assert.True(service.IsTokenRevoked(token));
        }

        [Fact]
        public void IsTokenRevoked_ReturnsFalse_ForUnrevokedToken()
        {
            // Arrange
            var service = new TokenService(GetOptions());
            var principal = GetPrincipal();
            var token = service.GenerateToken(principal);

            // Act
            var isRevoked = service.IsTokenRevoked(token);

            // Assert
            Assert.False(isRevoked);
        }

        [Fact]
        public void IsTokenRevoked_ReturnsFalse_ForMalformedToken()
        {
            // Arrange
            var service = new TokenService(GetOptions());
            var malformedToken = "not.a.valid.jwt";

            // Act
            var isRevoked = service.IsTokenRevoked(malformedToken);

            // Assert
            Assert.False(isRevoked);
        }
    }
}