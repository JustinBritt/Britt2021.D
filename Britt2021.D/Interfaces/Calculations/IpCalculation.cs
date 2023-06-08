namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    public interface IpCalculation
    {
        ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, FhirDecimal>> GenerateScenarios(
            INullableValueFactory nullableValueFactory,
            IDiscreteUniformFactory discreteUniformFactory,
            ImmutableList<INullableValue<int>> lengthOfStayDays,
            ImmutableList<INullableValue<int>> scenarios,
            Organization surgeon,
            ImmutableList<KeyValuePair<Organization, PositiveInt>> surgeonLengthOfStayMaximums,
            double targetMean);
    }
}