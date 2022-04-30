namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    public interface IρCalculation
    {
        ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> CalculateLogNormal(
            INullableValueFactory nullableValueFactory,
            ILogNormalFactory logNormalFactory,
            PositiveInt cluster,
            double µ,
            ImmutableList<PositiveInt> scenarios,
            double σ,
            Organization surgeon);
    }
}