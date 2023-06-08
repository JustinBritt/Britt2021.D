namespace Britt2021.D.Interfaces.Experiments
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Hl7.Fhir.Model;

    public interface IExperiment1
    {
        /// <summary>
        /// Gets the weekdays.
        /// Indices: d, d1, d2
        /// </summary>
        ImmutableList<INullableValue<int>> Weekdays { get; }

        /// <summary>
        /// Gets the surgical specialties.
        /// Index: j
        /// Used in: 3A, 3B, 5 
        /// </summary>
        ImmutableList<Tuple<Organization, ImmutableList<Organization>>> SurgicalSpecialties { get; }

        /// <summary>
        /// Gets the clusters.
        /// Index: k
        /// </summary>
        ImmutableList<INullableValue<int>> Clusters { get; }

        /// <summary>
        /// Gets the length of stay days.
        /// Parameter and Index: l, L(s)
        /// Used in: 4, 5
        /// </summary>
        ImmutableList<INullableValue<int>> LengthOfStayDays { get; }

        /// <summary>
        /// Gets the machines.
        /// Index: M
        /// Used in: 3A, 3B
        /// </summary>
        Bundle Machines { get; }

        /// <summary>
        /// Gets the operating rooms.
        /// Index: r
        /// Used in: All problems
        /// </summary>
        Bundle OperatingRooms { get; }

        /// <summary>
        /// Gets the surgeons.
        /// Index: s
        /// Used in: All problems
        /// </summary>
        Bundle Surgeons { get; }

        /// <summary>
        /// Gets the planning horizon.
        /// Index: t
        /// Used in: All problems
        /// </summary>
        ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> PlanningHorizon { get; }

        /// <summary>
        /// Gets the scenarios.
        /// Index: Λ
        /// Used in: 1B, 2, 4, 5
        /// </summary>
        ImmutableList<INullableValue<int>> Scenarios { get; }

        /// <summary>
        /// Gets the operating room service levels
        /// Index: υ1
        /// Used in: 1A, 1B
        /// </summary>
        ImmutableList<INullableValue<int>> OperatingRoomServiceLevels { get; }

        /// <summary>
        /// Gets SurgeonServiceLevelNumberTimeBlocks.
        /// Parameter: A(s, υ1)
        /// Used in: 1A, 1B
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> SurgeonServiceLevelNumberTimeBlocks { get; }

        /// <summary>
        /// Gets the machine costs.
        /// Parameter: C(m)
        /// Used in: 3A
        /// </summary>
        ImmutableList<KeyValuePair<Device, Money>> MachineCosts { get; }

        /// <summary>
        /// Gets the surgical frequencies.
        /// Parameter: f(s, k)
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> SurgicalFrequencies { get; }

        /// <summary>
        /// Gets the weighted average surgical durations.
        /// Parameter: h(i, Λ)
        /// Used in: 2
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> WeightedAverageSurgicalDurations { get; }

        /// <summary>
        /// Gets SurgeonLengthOfStayMaximums.
        /// Parameter and Index: L(s), l
        /// Used in: 4, 5 (indirectly)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, INullableValue<int>>> SurgeonLengthOfStayMaximums { get; }

        /// <summary>
        /// Gets SurgeonStrategicTargets.
        /// Parameter: N(s)
        /// Used in: 1B
        /// </summary>
        ImmutableList<KeyValuePair<Organization, INullableValue<int>>> SurgeonStrategicTargets { get; }

        /// <summary>
        /// Gets SurgeonScenarioMaximumNumberPatients.
        /// Parameter: n(s, Λ)
        /// Used in: 1B, 2
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> SurgeonScenarioMaximumNumberPatients { get; }

        /// <summary>
        /// Gets the service level probabilities.
        /// Parameter: P(υ1)
        /// Used in: 1A
        /// </summary>
        ImmutableList<KeyValuePair<INullableValue<int>, FhirDecimal>> ServiceLevelProbabilities { get; }

        /// <summary>
        /// Gets SurgeonDayScenarioLengthOfStayProbabilities.
        /// Parameter: p(s, l, Λ)
        /// Used in: 3B
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, FhirDecimal>> SurgeonDayScenarioLengthOfStayProbabilities { get; }

        /// <summary>
        /// Gets the number of days per week.
        /// Parameter: W
        /// Used in: 3B, 4, 5
        /// </summary>
        PositiveInt NumberDaysPerWeek { get; }

        /// <summary>
        /// Gets the time block length.
        /// Parameter: H
        /// Used in: 2
        /// </summary>
        Duration TimeBlockLength { get; }

        /// <summary>
        /// Gets SurgeonMachineRequirements.
        /// Parameter: ζ(s, m)
        /// Used in: 3A, 3B
        /// </summary>
        ImmutableList<Tuple<Organization, Device, FhirBoolean>> SurgeonMachineRequirements { get; }

        /// <summary>
        /// Gets the surgical overheads.
        /// Parameter: θ(s, k)
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> SurgicalOverheads { get; }

        /// <summary>
        /// Gets SurgeonScenarioMaximumNumberPatientMeans.
        /// Parameter: μ(s, Λ)
        /// Used in: 3B
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> SurgeonScenarioMaximumNumberPatientMeans { get; }

        /// <summary>
        /// Gets the scenario probabilities.
        /// Parameter: Ρ(Λ)
        /// Used in: 1B, 2, 4, 5
        /// </summary>
        ImmutableList<KeyValuePair<INullableValue<int>, FhirDecimal>> ScenarioProbabilities { get; }

        /// <summary>
        /// Gets the surgical durations.
        /// Parameter: ρ(s, k, Λ)
        /// </summary>
        ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> SurgicalDurations { get; }

        /// <summary>
        /// Gets SurgeonScenarioMaximumNumberPatientStandardDeviations.
        /// Parameter: σ(s, Λ)
        /// Used in: 3B
        /// </summary>
        ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> SurgeonScenarioMaximumNumberPatientStandardDeviations { get; }

        /// <summary>
        /// Gets DayAvailabilities.
        /// Parameter: ψ(t)
        /// Used in: 1A, 1B, 3A, 3B
        /// </summary>
        ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> DayAvailabilities { get; }

        /// <summary>
        /// Gets the maximum number of recovery ward beds.
        /// Parameter: Ω
        /// Used in: 3B
        /// </summary>
        PositiveInt MaximumNumberRecoveryWardBeds { get; }

        /// <summary>
        /// Gets SurgeonPenaltyWeights.
        /// Parameter: ω(s)
        /// Used in: 1B
        /// </summary>
        ImmutableList<KeyValuePair<Organization, FhirDecimal>> SurgeonPenaltyWeights { get; }

        /// <summary>
        /// Gets the length of stay days.
        /// Belien2007: d, d1, d2
        /// </summary>
        ImmutableList<PositiveInt> Belien2007LengthOfStayDays { get; }

        /// <summary>
        /// Gets the active periods.
        /// Belien2007: A
        /// </summary>
        ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> Belien2007ActivePeriods { get; }

        /// <summary>
        /// Gets DayNumberTimeBlocks.
        /// Belien2007: b(i)
        /// </summary>
        ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> Belien2007DayNumberTimeBlocks { get; }

        /// <summary>
        /// Gets DayBedCapacities.
        /// Belien2007: c(i)
        /// </summary>
        ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> Belien2007DayBedCapacities { get; }

        /// <summary>
        /// Gets SurgeonStateProbabilities.
        /// Belien2007: h(s, k)
        /// Used in: SMIP2
        /// </summary>
        ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> Belien2007SurgeonStateProbabilities { get; }

        /// <summary>
        /// Gets SurgeonLengthOfStayMaximums.
        /// Belien2007: m(s)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, PositiveInt>> Belien2007SurgeonLengthOfStayMaximums { get; }

        /// <summary>
        /// Gets SurgeonNumberStates.
        /// Belien2007: q(s)
        /// Used in: SMIP2
        /// </summary>
        ImmutableList<KeyValuePair<Organization, PositiveInt>> Belien2007SurgeonNumberStates { get; }

        /// <summary>
        /// Gets the mean weight.
        /// Belien2007: wMean
        /// Used in: SMIP2, MIP2
        /// </summary>
        FhirDecimal Belien2007MeanWeight { get; }

        /// <summary>
        /// Gets the variance weight.
        /// Belien2007: wVariance
        /// Used in: SMIP2, MIP2
        /// </summary>
        FhirDecimal Belien2007VarianceWeight { get; }

        /// <summary>
        /// Gets the active days.
        /// Ma2013: a
        /// </summary>
        ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> Ma2013ActiveDays { get; }

        /// <summary>
        /// Gets the block types.
        /// Ma2013: k
        /// </summary>
        ImmutableList<PositiveInt> Ma2013BlockTypes { get; }

        /// <summary>
        /// Gets WardSurgeonGroupPatientGroups.
        /// </summary>
        ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups { get; }

        /// <summary>
        /// Gets the patient groups.
        /// Ma2013: p
        /// </summary>
        ImmutableList<PositiveInt> Ma2013PatientGroups { get; }

        /// <summary>
        /// Gets BlockTypeTimeBlockLengths. 
        /// Ma2013: Length(k)
        /// </summary>
        ImmutableList<KeyValuePair<PositiveInt, Duration>> Ma2013BlockTypeTimeBlockLengths { get; }

        /// <summary>
        /// Gets DayOperatingRoomOperatingCapacities.
        /// Ma2013: ORday(a, r)
        /// </summary>
        ImmutableList<Tuple<FhirDateTime, Location, Duration>> Ma2013DayOperatingRoomOperatingCapacities { get; }

        /// <summary>
        /// Gets SurgeonGroupSubsetPatientGroups.
        /// Ma2013: P(s)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, PositiveInt>> Ma2013SurgeonGroupSubsetPatientGroups { get; }

        /// <summary>
        /// Gets WardSubsetPatientGroups.
        /// Ma2013: P(w)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, PositiveInt>> Ma2013WardSubsetPatientGroups { get; }

        /// <summary>
        /// Gets Wardα.
        /// Ma2013: α(w)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardα { get; }

        /// <summary>
        /// Gets Wardβ.
        /// Ma2013: β(w)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardβ { get; }

        /// <summary>
        /// Gets Wardγ.
        /// Ma2013: γ(w)
        /// </summary>
        ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardγ { get; }

        FhirDecimal GetBelien2007MeanWeightSMIP1();

        FhirDecimal GetBelien2007VarianceWeightSMIP1();

        // Parameter: p(s, d)
        ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> GetBelien2007SurgeonDayLengthOfStayProbabilities(
            PositiveInt scenario);

        ImmutableList<KeyValuePair<PositiveInt, Duration>> GetMa2013PatientGroupSurgeryDurations(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> surgicalDurations,
            ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> surgicalOverheads);

        ImmutableList<KeyValuePair<PositiveInt, PositiveInt>> GetMa2013PatientGroupThroughputsEvenlyDistributed(
            ImmutableList<KeyValuePair<Organization, PositiveInt>> HM1BSurgeonNumberAssignedTimeBlocks,
            ImmutableList<KeyValuePair<PositiveInt, Duration>> Ma2013PatientGroupSurgeryDurations,
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<KeyValuePair<Organization, PositiveInt>> surgeonStrategicTargets);

        ImmutableList<Tuple<PositiveInt, PositiveInt, FhirDecimal>> GetMa2013PatientGroupDayLengthOfStayProbabilities(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> surgeonDayScenarioLengthOfStayProbabilities);
    }
}