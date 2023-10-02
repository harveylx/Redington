using ProbabilityCalculator.API.Infrastructure;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using Xunit;

namespace ProbabilityCalculator.Tests.Infrastructure;

public class FileLoggingRepositoryTests
{
    [Fact]
    public async Task Log_WritesExpectedTextToFile()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var repository = new FileLoggingRepository(mockFileSystem);
        var operation = "CombinedWith";
        const decimal a = 0.5M;
        const decimal b = 0.3M;
        const decimal result = 0.15M;

        // Act
        await repository.Log(operation, a, b, result);

        // Assert
        var fileText = await mockFileSystem.File.ReadAllTextAsync("calculations.txt");
        fileText.Should().Contain($"Operation: {operation}");
        fileText.Should().Contain($"ProbabilityA: {decimal.Round(a, 3)}");
        fileText.Should().Contain($"ProbabilityB: {decimal.Round(b, 3)}");
        fileText.Should().Contain($"Result: {decimal.Round(result, 3)}");
    }
}