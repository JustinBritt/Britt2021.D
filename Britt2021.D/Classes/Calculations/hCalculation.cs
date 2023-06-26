namespace Britt2021.D.Classes.Calculations
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    using log4net;

    using Hl7.Fhir.Model;

    using NGenerics.DataStructures.Trees;

    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    
    internal sealed class hCalculation : IhCalculation
    {
        private ILog Log => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public hCalculation()
        {
        }

        public ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> Calculate(
            IDurationFactory durationFactory,
            ImmutableSortedSet<INullableValue<int>> clusters,
            Bundle surgeons,
            ImmutableSortedSet<INullableValue<int>> scenarios,
            RedBlackTree<Organization, RedBlackTree<INullableValue<int>, INullableValue<decimal>>> f,
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> θ,
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>> ρ)
        {
            return surgeons.Entry
                .Where(x => x.Resource is Organization)
                .Select(x => (Organization)x.Resource)
                .SelectMany(b => scenarios, (a, b) => Tuple.Create(a, b))
                .Select(i => Tuple.Create(
                    i.Item1, // i.Item1: Surgeon
                    i.Item2, // i.Item2: Scenario
                    durationFactory.CreateHour(
                        value: clusters
                        .Select(
                            k => // k: Cluster
                            ρ.Where(j => j.Item1 == i.Item1 && j.Item2 == k && j.Item3 == i.Item2)
                            .Select(j => j.Item4.Value.Value)
                            .SingleOrDefault()
                            *
                            θ.Where(j => j.Item1 == i.Item1 && j.Item2 == k)
                            .Select(j => j.Item3.Value.Value)
                            .SingleOrDefault()
                            *
                            f[i.Item1][i.Item2].Value.Value)
                        .Sum())))
                .ToImmutableList();
        }
    }
}