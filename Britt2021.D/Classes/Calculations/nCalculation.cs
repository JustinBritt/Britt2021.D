﻿namespace Britt2021.D.Classes.Calculations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    using log4net;

    using Hl7.Fhir.Model;

    using NGenerics.DataStructures.Trees;

    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    internal sealed class nCalculation : InCalculation
    {
        private ILog Log => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public nCalculation()
        {
        }

        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> Calculate(
            INullableValueFactory nullableValueFactory,
            RedBlackTree<Organization, RedBlackTree<INullableValue<int>, Duration>> h,
            Duration Η)
        {
            List<Tuple<Organization, INullableValue<int>, INullableValue<int>>> list = new();

            foreach (Organization surgeon in h.Keys.DistinctBy(w => w.Id))
            {
                foreach (INullableValue<int> scenario in h[surgeon].Keys.DistinctBy(w => w.Value.Value))
                {
                    list.Add(
                        Tuple.Create(
                            surgeon,
                            scenario,
                            nullableValueFactory.Create<int>(
                                (int)Math.Floor(
                                    Η.Value.Value
                                    /
                                    h[surgeon][scenario].Value.Value))));
                }
            }

            return list.ToImmutableList();
        }
    }
}