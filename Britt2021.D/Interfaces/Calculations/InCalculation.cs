namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    public interface InCalculation
    {
        ImmutableList<Tuple<Organization, PositiveInt, PositiveInt>> Calculate(
            INullableValueFactory nullableValueFactory,
            ImmutableList<Tuple<Organization, PositiveInt, Duration>> h,
            Duration Η);
    }
}