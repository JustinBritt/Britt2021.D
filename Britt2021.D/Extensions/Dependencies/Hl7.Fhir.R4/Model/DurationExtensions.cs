﻿namespace Britt2021.D.Extensions.Dependencies.Hl7.Fhir.R4.Model
{
    using System;

    using global::Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    public static class DurationExtensions
    {
        private const string minute = "min";
        private const string unitsofmeasure = "http://unitsofmeasure.org";

        public static Duration ToHour(
            this Duration duration,
            IDurationFactory durationFactory)
        {
            return duration.Unit switch
            {
                minute => durationFactory.Create(
                    unit: unitsofmeasure,
                    value: duration.Value.Value * (decimal)Math.Pow(60, -1)),

                _ => duration
            };
        }
    }
}