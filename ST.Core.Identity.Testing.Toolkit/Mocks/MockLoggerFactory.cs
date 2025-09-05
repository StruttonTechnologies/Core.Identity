using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Testing.Toolkit.Mocks
{
    /// <summary>
    /// Factory for creating mock ILogger instances.
    /// </summary>
    public static class MockLoggerFactory
    {
        public static ILogger<T> Create<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }
    }
}
