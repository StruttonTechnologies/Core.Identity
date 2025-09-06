using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Mapping;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Dtos.Authentication;
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
            var roles = new[] { "Admin", "User" };
            var token = "jwt-token";
            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, roles[0]),
            new Claim(ClaimTypes.Role, roles[1])
        };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var dto = principal.ToDto(token, expiresAt);

            // Assert
            Assert.True(dto.IsAuthenticated);
            Assert.Equal(token, dto.Token);
            Assert.Equal(expiresAt, dto.ExpiresAt);
            Assert.Equal(roles, dto.Roles);
            Assert.Equal(userId, dto.UserId);
            Assert.Equal(username, dto.Username);
        }

        [Fact]
        public void ToDto_ReturnsDefaults_WhenClaimsMissing()
        {
            // Arrange
            var token = "jwt-token";
            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var identity = new ClaimsIdentity(); // No claims, not authenticated
            var principal = new ClaimsPrincipal(identity);

            // Act
            var dto = principal.ToDto(token, expiresAt);

            // Assert
            Assert.False(dto.IsAuthenticated);
            Assert.Equal(token, dto.Token);
            Assert.Equal(expiresAt, dto.ExpiresAt);
            Assert.Empty(dto.Roles);
            Assert.Equal(Guid.Empty, dto.UserId);
            Assert.Equal(string.Empty, dto.Username);
        }

        [Fact]
        public void ToToken_ReturnsValidJwtToken()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var username = "testuser";
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes("supersecretkey1234567890supersecretkey"));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey1234567890supersecretkey"));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var options = new JwtTokenOptions(
                issuer: "TestIssuer",
                audience: "TestAudience",
                key: "supersecretkey1234567890supersecretkey",
                credentials: creds,
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
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        [Fact]
        public void ToToken_Throws_WhenNoClaimsIdentity()
        {
            // Arrange
            var principal = new ClaimsPrincipal(); // No identity
            var options = new JwtTokenOptions("issuer", "aud", "key", null, 60);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => principal.ToToken(options));
        }

        [Fact]
        public void ToToken_Throws_WhenNoClaims()
        {
            // Arrange
            var identity = new ClaimsIdentity(); // No claims
            var principal = new ClaimsPrincipal(identity);
            var options = new JwtTokenOptions("issuer", "aud", "key", null, 60);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => principal.ToToken(options));
        }
    } 
}