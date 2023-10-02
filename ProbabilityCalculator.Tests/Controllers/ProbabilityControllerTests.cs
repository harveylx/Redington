using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProbabilityCalculator.API.Controllers;
using ProbabilityCalculator.API.Infrastructure;
using ProbabilityCalculator.API.Models;
using ProbabilityCalculator.API.Services;
using ProbabilityCalculator.API.Validators;
using ProbabilityCalculator.Tests.Configuration;
using System.Linq.Expressions;
using Xunit;

namespace ProbabilityCalculator.Tests.Controllers;

public class ProbabilityControllerTests
{
    private readonly Mock<ICalculationService> _calculationServiceMock = new();
    private readonly Mock<ILoggingRepository> _loggingRepositoryMock = new();
    private readonly IValidator<Probability> _probabilityValidator = new ProbabilityInputValidator();

    [Theory, ProbabilityAutoData]
    public async Task CombinedWith_ValidProbabilities_ReturnsExpectedResult(
        decimal probabilityA, decimal probabilityB)
    {
        await ValidProbabilities_ReturnsExpectedResult(
            probabilityA, 
            probabilityB,
            (controller, input) => controller.CombinedWith(input),
            x => x.CombinedWith(probabilityA, probabilityB));
    }

    [Theory, ProbabilityAutoData]
    public async Task Either_ValidProbabilities_ReturnsExpectedResult(
        decimal probabilityA, decimal probabilityB)
    {
        await ValidProbabilities_ReturnsExpectedResult(
            probabilityA,
            probabilityB,
            (controller, input) => controller.Either(input),
            x => x.Either(probabilityA, probabilityB));
    }

    [Theory]
    [InlineData(1.1, 0.5)]
    [InlineData(-0.1, 0.5)]
    [InlineData(0.5, 1.1)]
    [InlineData(0.5, -0.1)]
    public async Task CombinedWith_InvalidProbability_ReturnsBadRequestResult(
        decimal probabilityA, decimal probabilityB)
    {
        await InvalidProbability_ReturnsBadRequestResult(probabilityA, probabilityB,
            (controller, input) => controller.CombinedWith(input));
    }

    [Theory] 
    [InlineData(1.1, 0.5)]
    [InlineData(-0.1, 0.5)]
    [InlineData(0.5, 1.1)]
    [InlineData(0.5, -0.1)]
    public async Task Either_InvalidProbability_ReturnsBadRequestResult(
        decimal probabilityA, decimal probabilityB)
    {
        await InvalidProbability_ReturnsBadRequestResult(probabilityA, probabilityB,
            (controller, input) => controller.Either(input));
    }

    private async Task ValidProbabilities_ReturnsExpectedResult(
        decimal probabilityA,
        decimal probabilityB,
        Func<ProbabilityController, Probability, Task<IActionResult>> action,
        Expression<Action<ICalculationService>> verifyExpression)
    {
        // Arrange
        var input = new Probability { ProbabilityA = probabilityA, ProbabilityB = probabilityB };
        var controller = new ProbabilityController(_calculationServiceMock.Object, _loggingRepositoryMock.Object,
            _probabilityValidator);

        // Act
        var result = await action(controller, input);

        // Assert
        _calculationServiceMock.Verify(verifyExpression, Times.Once);
        _loggingRepositoryMock.Verify(x => x.Log(It.IsAny<string>(), probabilityA, probabilityB, It.IsAny<decimal>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
    }


    private async Task InvalidProbability_ReturnsBadRequestResult(
        decimal invalidProbabilityA,
        decimal invalidProbabilityB,
        Func<ProbabilityController, Probability, Task<IActionResult>> action)
    {
        // Arrange
        var input = new Probability { ProbabilityA = invalidProbabilityA, ProbabilityB = invalidProbabilityB };
        var controller = new ProbabilityController(_calculationServiceMock.Object, _loggingRepositoryMock.Object,
            _probabilityValidator);

        // Ensure the probabilities are invalid.
        input.ProbabilityA = 1.1M;
        input.ProbabilityB = -0.1M;

        // Act
        var result = await action(controller, input);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

}