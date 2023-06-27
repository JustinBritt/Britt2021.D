namespace Britt2021.D.Interfaces.Calculations
{
    using System;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    using NGenerics.DataStructures.Trees;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    public interface IhCalculation
    {
        ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> Calculate(
            IDurationFactory durationFactory,
            ImmutableSortedSet<INullableValue<int>> clusters,
            Bundle surgeons,
            ImmutableSortedSet<INullableValue<int>> scenarios,
            RedBlackTree<Organization, RedBlackTree<INullableValue<int>, INullableValue<decimal>>> f,
            RedBlackTree<Organization, RedBlackTree<INullableValue<int>, INullableValue<decimal>>> θ,
            RedBlackTree<Organization, RedBlackTree<INullableValue<int>, RedBlackTree<INullableValue<int>, INullableValue<decimal>>>> ρ);
    }
}