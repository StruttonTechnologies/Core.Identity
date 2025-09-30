using System;
using System.Collections.Generic;
using System.Linq;

namespace ST.Core.Identity
{
    public class AggregateValidationException : Exception
    {
        public IReadOnlyList<Exception> InnerExceptions { get; }

        public AggregateValidationException(IEnumerable<Exception> innerExceptions, string message = "One or more validation checks failed.")
            : base(message)
        {
            InnerExceptions = innerExceptions.ToList().AsReadOnly();
        }

        /// <summary>
        /// Executes a series of deferred validation checks and throws a single aggregated exception if any fail.
        /// </summary>
        public static void Aggregate(params Action[] checks)
        {
            Aggregate("One or more validation checks failed.", checks);
        }

        /// <summary>
        /// Executes a series of deferred validation checks and throws a single aggregated exception with a custom message if any fail.
        /// </summary>
        /// <param name="message">The message for the aggregated exception.</param>
        /// <param name="checks">Validation checks to run.</param>
        public static void Aggregate(string message, params Action[] checks)
        {
            if (checks == null || checks.Length == 0)
                return;

            var exceptions = new List<Exception>();

            foreach (var check in checks)
            {
                try
                {
                    check(); // Execute the lambda
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateValidationException(exceptions, message);
        }
    }
}
