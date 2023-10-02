namespace ProbabilityCalculator.API.Validators;

using FluentValidation;
using Models;

public class ProbabilityInputValidator : AbstractValidator<Probability>
{
    public ProbabilityInputValidator()
    {
        RuleFor(x => x.ProbabilityA).InclusiveBetween(0, 1).PrecisionScale(4, 3, true);
        RuleFor(x => x.ProbabilityB).InclusiveBetween(0, 1).PrecisionScale(4, 3, true);
    }
}