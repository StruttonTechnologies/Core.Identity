using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.Logging;

using StruttonTechnologies.Core.Identity.Fakes.Factories;

namespace StruttonTechnologies.Core.Identity.Fakes.Tests.Factories
{
    [ExcludeFromCodeCoverage]
    public class MockLoggerFactoryTests
    {
        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(MockLoggerFactoryTests))]
        public void Create_ReturnsILoggerOfType(Type type)
        {
            object logger = CreateLoggerOfType(type);
            Assert.NotNull(logger);
            Assert.IsType(typeof(ILogger<>).MakeGenericType(type), logger, exactMatch: false);
        }

        [Fact]
        public void Create_MultipleCalls_ReturnDistinctInstances()
        {
            ILogger<string> logger1 = MockLoggerFactory.Create<string>();
            ILogger<string> logger2 = MockLoggerFactory.Create<string>();

            Assert.NotNull(logger1);
            Assert.NotNull(logger2);
            Assert.NotSame(logger1, logger2);
        }

        private static object CreateLoggerOfType(Type type)
        {
            MethodInfo method = typeof(MockLoggerFactory).GetMethod("Create")!.MakeGenericMethod(type);
            return method.Invoke(null, null)!;
        }
    }
}
