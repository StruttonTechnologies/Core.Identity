using Microsoft.Extensions.Logging;
using ST.Core.Identity.Testing.Setup.Factories;
using Xunit;

namespace ST.Core.Identity.Testing.Tests.Setup.Factories
{
    /// <summary>
    /// Unit tests for <see cref="MockLoggerFactory"/>.
    /// Validates mock logger creation for test-safe injection.
    /// </summary>
    public class MockLoggerFactoryTests
    {
        [Fact]
        public void Create_ReturnsMockLoggerInstance()
        {
            var logger = MockLoggerFactory.Create<object>();

            Assert.NotNull(logger);
            Assert.IsAssignableFrom<ILogger<object>>(logger);
        }
    }
}
