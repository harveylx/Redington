namespace ProbabilityCalculator.API.Services;

public class CalculationService : ICalculationService
{
    public decimal CombinedWith(decimal a, decimal b)
    {
        return a * b;
    }

    public decimal Either(decimal a, decimal b)
    {
        return a + b - a * b;
    }
}