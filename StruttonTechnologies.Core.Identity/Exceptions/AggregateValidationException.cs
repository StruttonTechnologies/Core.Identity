namespace StruttonTechnologies.Core.Identity
{
    public class AggregateValidationException : Exception
    {
        public AggregateValidationException()
            : base("One or more validation checks failed.")
        {
            InnerExceptions = Array.Empty<Exception>();
        }

        public AggregateValidationException(string message)
            : base(message)
        {
            InnerExceptions = Array.Empty<Exception>();
        }

        public AggregateValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            InnerExceptions = Array.Empty<Exception>();
        }

        public AggregateValidationException(IEnumerable<Exception> innerExceptions, string message = "One or more validation checks failed.")
            : base(message)
        {
            InnerExceptions = innerExceptions.ToList().AsReadOnly();
        }

        public IReadOnlyList<Exception> InnerExceptions { get; }

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
            {
                return;
            }

            List<Exception> exceptions = [];

            foreach (Action check in checks)
            {
                try
                {
                    check(); // Execute the lambda
                }
#pragma warning disable CA1031 // Do not catch general exception types - intentional to aggregate all validation failures
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateValidationException(exceptions, message);
            }
        }
    }
}
