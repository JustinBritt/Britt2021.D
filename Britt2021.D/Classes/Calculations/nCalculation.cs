namespace Britt2021.D.Classes.Calculations
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    using log4net;

    using Hl7.Fhir.Model;

    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    internal sealed class nCalculation : InCalculation
    {
        private ILog Log => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public nCalculation()
        {
        }

        public ImmutableList<Tuple<Organization, INullableValue<int>, PositiveInt>> Calculate(
            INullableValueFactory nullableValueFactory,
            ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> h,
            Duration Η)
        {
            return h
                .Select(i => Tuple.Create(
                    i.Item1,
                    i.Item2,
                    (PositiveInt)nullableValueFactory.Create<int>(
                        (int)Math.Floor(
                            Η.Value.Value
                            /
                            i.Item3.Value.Value))))
                .ToImmutableList();
        }
    }
}