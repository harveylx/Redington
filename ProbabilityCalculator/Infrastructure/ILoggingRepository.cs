namespace ProbabilityCalculator.API.Infrastructure;

public interface ILoggingRepository
{
    Task Log(string operation, decimal a, decimal b, decimal result);
}