namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.AggregateValidationException"/>.
    /// </summary>
    public class AggregateValidationExceptionTests
    {
        [Fact]
        public void Aggregate_WithPassingChecks_DoesNotThrow()
        {
            StruttonTechnologies.Core.Identity.AggregateValidationException.Aggregate(() => { }, () => { });
        }

        [Fact]
        public void Aggregate_WithFailingChecks_ThrowsAggregateValidationException()
        {
            AggregateValidationException exception = Assert.Throws<StruttonTechnologies.Core.Identity.AggregateValidationException>(() =>
                StruttonTechnologies.Core.Identity.AggregateValidationException.Aggregate(
                    () => throw new InvalidOperationException("one"),
                    () => throw new ArgumentException("two")));

            Assert.Equal(2, exception.InnerExceptions.Count);
        }
    }
}
