using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

using Moq;

namespace StruttonTechnologies.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating mock ILogger instances.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MockLoggerFactory
    {
        public static ILogger<T> Create<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }
    }
}
