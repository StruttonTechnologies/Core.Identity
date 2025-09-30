using ST.Core.Identity.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Gaurd
{
    /// <summary>
    /// Orchestrates multiple guard checks and throws a single aggregated exception if any fail.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Executes a series of synchronous guard checks.
        /// Collects exceptions and throws an AggregateValidationException if any fail.
        /// </summary>
        /// <param name="checks">A list of actions that may throw exceptions.</param>
        public static void Execute(params Action[] checks)
        {
            var exceptions = new List<Exception>();

            foreach (var check in checks)
            {
                try
                {
                    check();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateValidationException(exceptions);
        }

        /// <summary>
        /// Executes a series of asynchronous guard checks.
        /// Collects exceptions and throws an AggregateValidationException if any fail.
        /// </summary>
        /// <param name="checks">A list of async functions that may throw exceptions.</param>
        public static async Task ExecuteAsync(params Func<Task>[] checks)
        {
            var exceptions = new List<Exception>();

            foreach (var check in checks)
            {
                try
                {
                    await check();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateValidationException(exceptions);
        }
    }
}
