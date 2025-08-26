using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Application.Authentication.Models
{
    /// <summary>
    /// Represents options for generating JWT tokens.
    /// </summary>
    /// 
    [ExcludeFromCodeCoverage]
    public class JwtTokenOptions
    {
        /// <summary>
        /// Gets the issuer of the JWT token.
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// Gets the audience for the JWT token.
        /// </summary>
        public string Audience { get; }

        /// <summary>
        /// Gets the security key used to sign the JWT token.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the signing credentials for the JWT token.
        /// </summary>
        public SigningCredentials Credentials { get; }

        /// <summary>
        /// Gets the number of minutes until the token expires.
        /// </summary>
        public int ExpirationMinutes { get; }


        /// <summary>
        /// Gets the expiration date and time for the JWT token.
        /// </summary>
        public DateTime ExpiresAt { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenOptions"/> class.
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience of the token.</param>
        /// <param name="key">The security key for signing the token.</param>
        /// <param name="credentials">The signing credentials.</param>
        /// <param name="expiresAt">The expiration date and time.</param>
        public JwtTokenOptions(string issuer, string audience, string key, SigningCredentials credentials, int expirationMinutes )
        {
            Issuer = issuer;
            Audience = audience;
            Key = key;
            Credentials = credentials;
            ExpirationMinutes = expirationMinutes;
        }
    }
}