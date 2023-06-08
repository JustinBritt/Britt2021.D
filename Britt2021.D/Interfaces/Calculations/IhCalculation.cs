﻿namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    public interface IhCalculation
    {
        ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> Calculate(
            IDurationFactory durationFactory,
            ImmutableList<INullableValue<int>> clusters,
            Bundle surgeons,
            ImmutableList<INullableValue<int>> scenarios,
            ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> f,
            ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> θ,
            ImmutableList<Tuple<Organization, INullableValue<int>, PositiveInt, FhirDecimal>> ρ);
    }
}