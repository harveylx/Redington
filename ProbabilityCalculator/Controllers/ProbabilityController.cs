using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProbabilityCalculator.API.Infrastructure;
using ProbabilityCalculator.API.Models;
using ProbabilityCalculator.API.Services;

namespace ProbabilityCalculator.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProbabilityController : ControllerBase
{
    private readonly ICalculationService _calculationService;
    private readonly ILoggingRepository _loggingRepository;
    private readonly IValidator<Probability> _probabilityValidator;

    public ProbabilityController(ICalculationService calculationService,
        ILoggingRepository loggingRepository,
        IValidator<Probability> probabilityValidator)
    {
        _calculationService = calculationService;
        _loggingRepository = loggingRepository;
        _probabilityValidator = probabilityValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CombinedWith([FromBody] Probability input)
    {
        var validationResult = _probabilityValidator.Validate(input);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var result = _calculationService.CombinedWith(input.ProbabilityA, input.ProbabilityB);

        await _loggingRepository.Log(nameof(_calculationService.CombinedWith), input.ProbabilityA, input.ProbabilityB, result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Either([FromBody] Probability input)
    {
        var validationResult = _probabilityValidator.Validate(input);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var result = _calculationService.Either(input.ProbabilityA, input.ProbabilityB);

        await _loggingRepository.Log(nameof(_calculationService.Either), input.ProbabilityA, input.ProbabilityB, result);

        return Ok(result);
    }
}
