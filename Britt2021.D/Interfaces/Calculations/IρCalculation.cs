namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    public interface IρCalculation
    {
        ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, FhirDecimal>> CalculateLogNormal(
            INullableValueFactory nullableValueFactory,
            ILogNormalFactory logNormalFactory,
            INullableValue<int> cluster,
            double µ,
            ImmutableList<INullableValue<int>> scenarios,
            double σ,
            Organization surgeon);
    }
}