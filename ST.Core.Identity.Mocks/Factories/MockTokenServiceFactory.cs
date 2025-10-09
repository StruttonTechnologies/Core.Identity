using Moq;
using ST.Core.Identity.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating a mocked ITokenService.
    /// Used for application-level JWT or API token scenarios.
    /// </summary>
    public static class MockTokenServiceFactory
    {
        public static Mock<ITokenService> Create()
        {
            var mock = new Mock<ITokenService>();

            // Always return a known valid token for generation
            mock.Setup(s => s.GenerateToken(It.IsAny<string>()))
                .Returns(KnownTokens.ValidToken);

            // Validation rules based on KnownTokens
            mock.Setup(s => s.ValidateToken(KnownTokens.ValidToken))
                .Returns(true);

            mock.Setup(s => s.ValidateToken(KnownTokens.ExpiredToken))
                .Returns(false);

            mock.Setup(s => s.ValidateToken(KnownTokens.InvalidToken))
                .Returns(false);

            return mock;
        }
    }

    /// <summary>
    /// Example token service abstraction.
    /// </summary>
    public interface ITokenService
    {
        string GenerateToken(string userId);
        bool ValidateToken(string token);
    }
}
