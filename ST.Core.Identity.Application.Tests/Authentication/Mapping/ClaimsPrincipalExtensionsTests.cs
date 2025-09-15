using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Mapping;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Dtos.Authentication.Logins;
using Xunit;

namespace ST.Core.Identity.Application.Tests.Authentication.Mapping
{
    public class ClaimsPrincipalExtensionsTests
    {
        [Fact]
        public void ToDto_ReturnsExpectedDto_WithValidClaims()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var provider = "Google";
            var isNewUser = true;
            var requiresTwoFactor = true;
            var accessToken = "access-token";
            var refreshToken = "refresh-token";
            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim("provider", provider),
                new Claim("is_new_user", isNewUser.ToString().ToLowerInvariant()),
                new Claim("requires_2fa", requiresTwoFactor.ToString().ToLowerInvariant())
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var dto = principal.ToDto(accessToken, refreshToken, expiresAt);

            // Assert
            Assert.Equal(accessToken, dto.AccessToken);
            Assert.Equal(refreshToken, dto.RefreshToken);
            Assert.Equal(expiresAt, dto.ExpiresAt);
            Assert.Equal(userId, dto.UserId);
            Assert.Equal(username, dto.Username);
            Assert.Equal(provider, dto.Provider);
            Assert.True(dto.IsNewUser);
            Assert.True(dto.RequiresTwoFactor);
        }

        [Fact]
        public void ToDto_ReturnsDefaults_WhenClaimsMissing()
        {
            // Arrange
            var accessToken = "access-token";
            var refreshToken = "refresh-token";
            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var principal = new ClaimsPrincipal(new ClaimsIdentity()); // No claims

            // Act
            var dto = principal.ToDto(accessToken, refreshToken, expiresAt);

            // Assert
            Assert.Equal(accessToken, dto.AccessToken);
            Assert.Equal(refreshToken, dto.RefreshToken);
            Assert.Equal(expiresAt, dto.ExpiresAt);
            Assert.Equal(Guid.Empty, dto.UserId);
            Assert.Equal(string.Empty, dto.Username);
            Assert.Equal("Unknown", dto.Provider);
            Assert.False(dto.IsNewUser);
            Assert.False(dto.RequiresTwoFactor);
        }

        [Fact]
        public void ToToken_ReturnsValidJwtToken()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var username = "testuser";
            var provider = "Google";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim("provider", provider),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            var key = "supersecretkey1234567890supersecretkey";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var options = new JwtTokenOptions(
                issuer: "TestIssuer",
                audience: "TestAudience",
                key: key,
                credentials: credentials,
                expirationMinutes: 60
            );

            // Act
            var jwt = principal.ToToken(options);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Equal("TestAudience", token.Audiences.First());
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Name && c.Value == username);
            Assert.Contains(token.Claims, c => c.Type == "provider" && c.Value == provider);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        [Fact]
        public void ToToken_Throws_WhenNoClaimsIdentity()
        {
            // Arrange
            var principal = new ClaimsPrincipal(); // No identity
            var options = new JwtTokenOptions("issuer", "audience", "key", null, 60);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => principal.ToToken(options));
            Assert.Equal("ClaimsPrincipal must have a ClaimsIdentity.", ex.Message);
        }

        [Fact]
        public void ToToken_Throws_WhenNoClaims()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity()); // No claims
            var options = new JwtTokenOptions("issuer", "audience", "key", null, 60);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => principal.ToToken(options));
            Assert.Equal("ClaimsPrincipal must contain claims to generate a token.", ex.Message);
        }
    }
}