namespace Britt2021.D.Classes.Calculations
{
    using System;
    using System.Collections.Immutable;

    using log4net;

    using Hl7.Fhir.Model;

    using MathNet.Numerics.Distributions;

    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    internal sealed class ρCalculation : IρCalculation
    {
        private ILog Log => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ρCalculation()
        {
        }

        public ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> CalculateLogNormal(
            INullableValueFactory nullableValueFactory,
            ILogNormalFactory logNormalFactory,
            PositiveInt cluster,
            double µ,
            ImmutableList<PositiveInt> scenarios,
            double σ,
            Organization surgeon)
        {
            ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>>();

            // https://stackoverflow.com/questions/48014712/get-lognormal-random-number-given-log10-mean-and-log10-standard-deviation/48016650#48016650
            if (µ != 0)
            {
                double normalσ = Math.Sqrt(
                Math.Log(
                    1.0d
                    +
                    Math.Pow((σ / µ), 2)));

                double normalµ = Math.Log(µ) - 0.5d * Math.Pow(normalσ, 2);

                IContinuousDistribution logNormal = logNormalFactory.Create(
                    µ: normalµ,
                    σ: normalσ);

                foreach (PositiveInt scenario in scenarios)
                {
                    builder.Add(Tuple.Create(
                        surgeon,
                        cluster,
                        scenario,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            (decimal)logNormal.Sample())));
                }
            }
            else
            {
                foreach (PositiveInt scenario in scenarios)
                {
                    builder.Add(Tuple.Create(
                        surgeon,
                        cluster,
                        scenario,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            0m)));
                }
            }
            
            return builder.ToImmutableList();
        }
    }
}