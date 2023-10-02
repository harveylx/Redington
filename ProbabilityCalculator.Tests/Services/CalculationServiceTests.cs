using ProbabilityCalculator.API.Services;
using FluentAssertions;
using Xunit;

namespace ProbabilityCalculator.Tests.Services;

public class CalculationServiceTests
{
    private readonly CalculationService _calculationService = new();

    [Theory]
    [InlineData(0.5, 0.5, 0.25)]
    [InlineData(1, 0.5, 0.5)]
    [InlineData(0, 0.5, 0)]
    public void CombinedWith_ValidInputs_ReturnsExpectedResult(decimal a, decimal b, decimal expectedResult)
    {
        // Act
        var result = _calculationService.CombinedWith(a, b);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0.5, 0.5, 0.75)]
    [InlineData(1, 0.5, 1)]
    [InlineData(0, 0.5, 0.5)]
    public void Either_ValidInputs_ReturnsExpectedResult(decimal a, decimal b, decimal expectedResult)
    {
        // Act
        var result = _calculationService.Either(a, b);

        // Assert
        result.Should().Be(expectedResult);
    }
}