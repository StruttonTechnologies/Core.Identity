using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Tests.Core.Exceptions;

/// <summary>
/// Contains test scenarios for <see cref="AggregateValidationException"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class AggregateValidationExceptionTests
{
    [Fact]
    public void Aggregate_WithPassingChecks_DoesNotThrow()
    {
        AggregateValidationException.Aggregate(() => { }, () => { });
    }

    [Fact]
    public void Aggregate_WithFailingChecks_ThrowsAggregateValidationException()
    {
        AggregateValidationException exception = Assert.Throws<AggregateValidationException>(() =>
            AggregateValidationException.Aggregate(
                () => throw new InvalidOperationException("one"),
                () => throw new ArgumentException("two")));

        Assert.Equal(2, exception.InnerExceptions.Count);
    }
}
