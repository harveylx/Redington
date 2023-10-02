using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using AutoFixture;
using System.Reflection;

namespace ProbabilityCalculator.Tests.Configuration;

public class ProbabilityAutoDataAttribute : AutoDataAttribute
{
    public ProbabilityAutoDataAttribute() : base(() =>
        new Fixture().Customize(new ProbabilityCustomization()))
    {
    }
}

public class ProbabilityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ProbabilitySpecimenBuilder());
    }
}

public class ProbabilitySpecimenBuilder : ISpecimenBuilder
{
    private readonly Random _random = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is ParameterInfo parameterInfo && parameterInfo.ParameterType == typeof(decimal))
        {
            // We do this to return a maximum of 3 decimal places
            return Math.Round((decimal)_random.NextDouble() * 1000m) / 1000m;
        }
        return new NoSpecimen();
    }
}
