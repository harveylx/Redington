namespace ProbabilityCalculator.API.Services;

public interface ICalculationService
{
    decimal CombinedWith(decimal a, decimal b);
    decimal Either(decimal a, decimal b);
}