namespace Britt2021.D.Classes.Experiments
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    
    using log4net;

    using Hl7.Fhir.Model;

    using MathNet.Numerics.Distributions;

    using Britt2021.D.Extensions.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.Interfaces.Experiments;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    public sealed class Experiment1 : IExperiment1
    {
        private ILog Log => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Experiment1(
            ICalculationsAbstractFactory calculationsAbstractFactory,
            IDependenciesAbstractFactory dependenciesAbstractFactory)
        {
            IBundleFactory bundleFactory = dependenciesAbstractFactory.CreateBundleFactory();

            IContinuousUniformFactory continuousUniformFactory = dependenciesAbstractFactory.CreateContinuousUniformFactory();

            this.DurationFactory = dependenciesAbstractFactory.CreateDurationFactory();

            IDeviceFactory deviceFactory = dependenciesAbstractFactory.CreateDeviceFactory();

            IEntryComponentFactory entryComponentFactory = dependenciesAbstractFactory.CreateEntryComponentFactory();

            this.FhirDateTimeFactory = dependenciesAbstractFactory.CreateFhirDateTimeFactory();

            ILocationFactory locationFactory = dependenciesAbstractFactory.CreateLocationFactory();

            IMoneyFactory moneyFactory = dependenciesAbstractFactory.CreateMoneyFactory();

            INullableValueFactory nullableValueFactory = dependenciesAbstractFactory.CreateNullableValueFactory();

            this.NullableValueFactory = nullableValueFactory;

            IOrganizationFactory organizationFactory = dependenciesAbstractFactory.CreateOrganizationFactory();

            IDiscreteUniformFactory discreteUniformFactory = dependenciesAbstractFactory.CreateDiscreteUniformFactory();

            ILogNormalFactory logNormalFactory = dependenciesAbstractFactory.CreateLogNormalFactory();

            IhCalculation hCalculation = calculationsAbstractFactory.CreatehCalculationFactory().Create();

            InCalculation nCalculation = calculationsAbstractFactory.CreatenCalculationFactory().Create();

            // Weekdays
            // Indices: d, d1, d2
            // Used in: 3B, 4, 5
            int numberWeekdaysPerWeek = 5;

            this.Weekdays = this.GenerateWeekdays(
                this.NullableValueFactory,
                numberWeekdaysPerWeek);

            // Clusters
            // Index: k
            // 8 Clusters in Van Houdenhoven et al. (2007)
            int numberClusters = 8;

            this.Clusters = this.GenerateClusters(
                this.NullableValueFactory,
                numberClusters);

            // Length of stay days
            // Parameter and Index: l, L(s)
            // Used in: 4, 5
            int maximumLengthOfStay = 5;

            this.LengthOfStayDays = this.GenerateLengthOfStayDays(
                nullableValueFactory,
                maximumLengthOfStay);

            // Machines
            // Index: M
            // Used in: 3A, 3B
            int numberMachines = 300;

            this.Machines = this.GenerateMachines(
                bundleFactory,
                deviceFactory,
                entryComponentFactory,
                numberMachines);

            // OperatingRooms
            // Index: r
            // Used in: All problems
            int numberOperatingRooms = 16;

            this.OperatingRooms = this.GenerateOperatingRooms(
                bundleFactory,
                entryComponentFactory,
                locationFactory,
                numberOperatingRooms);

            // Surgical Specialties

            // Surgical Specialty 1 - General / Mixed (GEN / MIX)
            this.SurgicalSpecialty1GEN = organizationFactory.Create(
                id: "1");

            // Surgical Specialty 2 - Gynecology (GYN)
            this.SurgicalSpecialty2GYN = organizationFactory.Create(
                id: "2");

            // Surgical Specialty 3 - Plastic (PLA)
            this.SurgicalSpecialty3PLA = organizationFactory.Create(
                id: "3");

            // Surgical Specialty 4 - Ear-Nose-Throat (ENT)
            this.SurgicalSpecialty4ENT = organizationFactory.Create(
                id: "4");

            // Surgical Specialty 5 - Orthopedic (ORT)
            this.SurgicalSpecialty5ORT = organizationFactory.Create(
                id: "5");

            // Surgical Specialty 6 - Ophthalmology / Eye (EYE)
            this.SurgicalSpecialty6EYE = organizationFactory.Create(
                id: "6");

            // Surgical Specialty 7 - Urology (URO)
            this.SurgicalSpecialty7URO = organizationFactory.Create(
                id: "7");

            // Surgeons
            // Index: s
            // Used in: All problems
            int numberSurgeons = 50;

            this.Surgeons = this.GenerateSurgeons(
                bundleFactory,
                entryComponentFactory,
                organizationFactory,
                numberSurgeons);

            // SurgicalSpecialties
            // Index: j
            // Used in: 3A, 3B, 5 
            this.SurgicalSpecialties = this.GenerateSurgicalSpecialties();

            // SurgeonLengthOfStayMaximums
            // Parameter and Index: L(s), l
            // Used in: 4, 5 (indirectly)
            this.SurgeonLengthOfStayMaximums = this.GenerateSurgeonLengthOfStayMaximumsSameForAllSurgeons(
                this.NullableValueFactory,
                maximumLengthOfStay,
                this.Surgeons);

            // Planning Horizon
            // Index: t
            // Used in: All problems
            int planningHorizonLength = 42;

            DateTime startDate = new DateTime(
                2020,
                6,
                1);

            DateTime endDate = startDate.AddDays(
                planningHorizonLength - 1);

            this.PlanningHorizon = this.GeneratePlanningHorizon(
                this.FhirDateTimeFactory,
                this.NullableValueFactory,
                endDate,
                startDate);

            // Scenarios
            // Index: Λ
            // Used in: 1B, 2, 4, 5
            int numberScenarios = 4;

            this.Scenarios = this.GenerateScenarios(
                nullableValueFactory,
                numberScenarios);

            // OperatingRoomServiceLevels
            // Index: υ1
            // Used in: 1A, 1B
            int numberOperatingRoomServiceLevels = 3;

            this.OperatingRoomServiceLevels = this.GenerateOperatingRoomServiceLevels(
                nullableValueFactory,
                numberOperatingRoomServiceLevels);

            // SurgeonServiceLevelNumberTimeBlocks
            // Parameter: A(s, υ1)
            // Used in: 1A, 1B
            // 16 operating rooms, 42 day planning horizon, no weekends: 8, 12, 16
            this.SurgeonServiceLevelNumberTimeBlocks = this.GenerateSurgeonServiceLevelNumberTimeBlocks(
                this.NullableValueFactory);

            // MachineCosts
            // Parameter: C(m)
            // Used in: 3A
            this.MachineCosts = this.GenerateMachineCostsSameCost(
                moneyFactory: moneyFactory,
                cost: 300,
                currency: Money.Currencies.CAD);

            // SurgicalDurations
            // Parameter: ρ(s, k, Λ)
            this.SurgicalDurations = this.GenerateSurgicalDurationsVanHoudenhoven2007(
                this.DurationFactory,
                nullableValueFactory,
                logNormalFactory,
                calculationsAbstractFactory.CreateρCalculationFactory().Create(),
                this.Clusters,
                this.Scenarios,
                this.Surgeons,
                this.SurgicalSpecialties);

            // SurgicalOverheads
            // Parameter: θ(s, k)
            this.SurgicalOverheads = this.GenerateSurgicalOverheads(
                nullableValueFactory,
                continuousUniformFactory,
                lower: 1.0,
                upper: 1.5);

            // SurgicalFrequencies
            // Parameter: f(s, k)
            this.SurgicalFrequencies = this.GenerateSurgicalFrequenciesVanHoudenhoven2007(
                this.Clusters,
                this.Surgeons,
                this.SurgicalSpecialties);

            // WeightedAverageSurgicalDurations
            // Parameter: h(i, Λ)
            // Used in: 2
            this.WeightedAverageSurgicalDurations = hCalculation.Calculate(
                this.DurationFactory,
                this.Clusters,
                this.Surgeons,
                this.Scenarios,
                this.SurgicalFrequencies,
                this.SurgicalOverheads,
                this.SurgicalDurations);

            // SurgeonStrategicTargets
            // Parameter: N(s)
            // Used in: 1B
            this.SurgeonStrategicTargets = this.GenerateSurgeonStrategicTargetsSameForAllSurgeons(
                this.NullableValueFactory);

            // TimeBlockLength
            // Parameter: H
            // Used in: 2
            this.TimeBlockLength = this.DurationFactory.CreateHour(
                value: 7.5m);

            // SurgeonScenarioMaximumNumberPatients
            // Parameter: n(s, Λ)
            // Used in: 1B, 2
            this.SurgeonScenarioMaximumNumberPatients = nCalculation.Calculate(
                this.NullableValueFactory,
                this.WeightedAverageSurgicalDurations,
                this.TimeBlockLength);

            // ServiceLevelProbabilities
            // Parameter: P(υ1)
            // Used in: 1A
            this.ServiceLevelProbabilities = this.GenerateServiceLevelProbabilities(
                this.NullableValueFactory);

            // SurgeonDayScenarioLengthOfStayProbabilities
            // Parameter: p(s, l, Λ)
            // Used in: 3B
            this.SurgeonDayScenarioLengthOfStayProbabilities = this.GenerateSurgeonDayScenarioLengthOfStayProbabilitiesVanOostrum2011(
                nullableValueFactory,
                discreteUniformFactory,
                calculationsAbstractFactory.CreatepCalculationFactory().Create(),
                this.Surgeons,
                this.SurgicalSpecialties);

            // NumberDaysPerWeek
            // Parameter: W
            // Used in: 3B, 4, 5
            int numberDaysPerWeek = 7;

            this.NumberDaysPerWeek = nullableValueFactory.Create<int>(
                numberDaysPerWeek);

            // SurgeonMachineRequirements
            // Parameter: ζ(s, m)
            // Used in: 3A, 3B
            this.SurgeonMachineRequirements = this.GenerateSurgeonMachineRequirements(
                this.NullableValueFactory);

            // SurgeonScenarioMaximumNumberPatientMeans
            // Parameter: μ(s, Λ)
            // Used in: 3B
            this.SurgeonScenarioMaximumNumberPatientMeans = this.GenerateSurgeonScenarioMaximumNumberPatientMeans(
                this.NullableValueFactory);

            // ScenarioProbabilities
            // Parameter: Ρ(Λ)
            // Used in: 1B, 2, 4, 5
            this.ScenarioProbabilities = this.GenerateScenarioProbabilities(
                nullableValueFactory);

            // SurgeonScenarioMaximumNumberPatientStandardDeviations
            // Parameter: σ(s, Λ)
            // Used in: 3B
            this.SurgeonScenarioMaximumNumberPatientStandardDeviations = this.GenerateSurgeonScenarioMaximumNumberPatientStandardDeviations(
                this.NullableValueFactory);

            // DayAvailabilities
            // Parameter: ψ(t)
            // Used in: 1A, 1B, 3A, 3B
            this.DayAvailabilities = this.GenerateDayAvailabilitiesAllOperatingRoomsUnavailableOnWeekends(
                this.FhirDateTimeFactory,
                this.NullableValueFactory,
                endDate,
                startDate);

            // MaximumNumberRecoveryWardBeds
            // Parameter: Ω
            // Used in: 3B
            this.MaximumNumberRecoveryWardBeds = (PositiveInt)nullableValueFactory.Create<int>(
                60);

            // SurgeonPenaltyWeights
            // Parameter: ω(s)
            // Used in: 1B
            this.SurgeonPenaltyWeights = this.GenerateSurgeonPenaltyWeightsSameForAllSurgeons(
                this.NullableValueFactory,
                this.Surgeons,
                2m);

            int Belien2007MaximumLengthOfStay = maximumLengthOfStay + 1;
            
            // Belien2007: d, d1, d2
            this.Belien2007LengthOfStayDays = this.GenerateBelien2007LengthOfStayDays(
                this.NullableValueFactory,
                Belien2007MaximumLengthOfStay);

            // Belien2007: A
            this.Belien2007ActivePeriods = this.GenerateBelien2007ActivePeriodsAllOperatingRoomsUnavailableOnWeekends(
                endDate,
                startDate);

            // Belien2007: b(i)
            this.Belien2007DayNumberTimeBlocks = this.GenerateBelien2007DayNumberTimeBlocks(
                this.NullableValueFactory,
                numberOperatingRooms);

            // Belien2007: c(i)
            this.Belien2007DayBedCapacities = this.GenerateBelien2007DayBedCapacities(
                this.Surgeons);

            // Belien2007: h(s, k)
            // Used in: SMIP2
            this.Belien2007SurgeonStateProbabilities = this.GenerateBelien2007SurgeonStateProbabilities(
                this.Surgeons);

            // Belien2007: m(s)
            this.Belien2007SurgeonLengthOfStayMaximums = this.GenerateBelien2007SurgeonLengthOfStayMaximumsSameForAllSurgeons(
                this.NullableValueFactory,
                Belien2007MaximumLengthOfStay,
                this.Surgeons);

            // Belien2007: q(s)
            // Used in: SMIP2
            this.Belien2007SurgeonNumberStates = this.GenerateBelien2007SurgeonNumberStates(
                this.NullableValueFactory);

            // Belien2007: wMean
            // Used in: SMIP2, MIP2
            this.Belien2007MeanWeight = (FhirDecimal)nullableValueFactory.Create<decimal>(
                0.25m);

            // Belien2007: wVariance
            // Used in: SMIP2, MIP2
            this.Belien2007VarianceWeight = (FhirDecimal)nullableValueFactory.Create<decimal>(
                0.75m);

            // Ma2013: a
            this.Ma2013ActiveDays = this.GenerateMa2013ActiveDaysAllOperatingRoomsUnavailableOnWeekends(
                this.PlanningHorizon);

            // Ma2013: k
            this.Ma2013BlockTypes = this.GenerateMa2013BlockTypesOnlyOneBlockType();

            // Ma2013: WardSurgeonGroupPatientGroups
            this.Ma2013WardSurgeonGroupPatientGroups = this.GenerateMa2013WardSurgeonGroupPatientGroups(
                this.NullableValueFactory,
                numberClusters,
                this.SurgicalSpecialties);

            // Ma2013: p
            this.Ma2013PatientGroups = this.GenerateMa2013PatientGroups(
                this.Ma2013WardSurgeonGroupPatientGroups);

            // Ma2013: Length(k)
            this.Ma2013BlockTypeTimeBlockLengths = this.GenerateMa2013BlockTypeTimeBlockLengthsOnlyOneBlockType(
                this.Ma2013BlockTypes,
                this.TimeBlockLength);

            // Ma2013: ORday(a, r)
            this.Ma2013DayOperatingRoomOperatingCapacities = this.GenerateMa2013DayOperatingRoomOperatingCapacities(
                this.Ma2013ActiveDays,
                this.OperatingRooms,
                this.TimeBlockLength);

            // Ma2013: P(s)
            this.Ma2013SurgeonGroupSubsetPatientGroups = this.GenerateMa2013SurgeonGroupSubsetPatientGroups(
                this.Ma2013WardSurgeonGroupPatientGroups);

            // Ma2013: P(w)
            this.Ma2013WardSubsetPatientGroups = this.GenerateMa2013WardSubsetPatientGroups(
                this.Ma2013WardSurgeonGroupPatientGroups);

            // Ma2013: α(w)
            this.Ma2013Wardα = this.GenerateMa2013Wardα(
                this.NullableValueFactory,
                this.Ma2013WardSurgeonGroupPatientGroups);

            // Ma2013: β(w)
            this.Ma2013Wardβ = this.GenerateMa2013Wardβ(
                this.NullableValueFactory,
                this.Ma2013WardSurgeonGroupPatientGroups);

            // Ma2013: γ(w)
            this.Ma2013Wardγ = this.GenerateMa2013Wardγ(
                this.NullableValueFactory,
                this.Ma2013WardSurgeonGroupPatientGroups);
        }

        /// <summary>
        /// Gets a factory can create <see cref="Duration"/> instances.
        /// </summary>
        public IDurationFactory DurationFactory { get; }

        /// <summary>
        /// Gets a factory can create <see cref="FhirDateTime"/> instances.
        /// </summary>
        public IFhirDateTimeFactory FhirDateTimeFactory { get; }

        /// <summary>
        /// Gets a factory can create <see cref="FhirBoolean"/>, <see cref="FhirDecimal"/>, and <see cref="PositiveInt"/> instances.
        /// </summary>
        public INullableValueFactory NullableValueFactory { get; }

        /// <inheritdoc />
        public ImmutableList<INullableValue<int>> Weekdays { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, ImmutableList<Organization>>> SurgicalSpecialties { get; }

        /// <inheritdoc />
        public ImmutableList<INullableValue<int>> Clusters { get; }

        /// <inheritdoc />
        public ImmutableList<INullableValue<int>> LengthOfStayDays { get; }

        /// <inheritdoc />
        public Bundle Machines { get; }

        /// <inheritdoc />
        public Bundle OperatingRooms { get; }

        /// <inheritdoc />
        public Bundle Surgeons { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> PlanningHorizon { get; }

        /// <inheritdoc />
        public ImmutableList<INullableValue<int>> Scenarios { get; }

        /// <inheritdoc />
        public ImmutableList<INullableValue<int>> OperatingRoomServiceLevels { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> SurgeonServiceLevelNumberTimeBlocks { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Device, Money>> MachineCosts { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> SurgicalFrequencies { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, Duration>> WeightedAverageSurgicalDurations { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, INullableValue<int>>> SurgeonLengthOfStayMaximums { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, INullableValue<int>>> SurgeonStrategicTargets { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> SurgeonScenarioMaximumNumberPatients { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<INullableValue<int>, INullableValue<decimal>>> ServiceLevelProbabilities { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>> SurgeonDayScenarioLengthOfStayProbabilities { get; }

        /// <inheritdoc />
        public INullableValue<int> NumberDaysPerWeek { get; }

        /// <inheritdoc />
        public Duration TimeBlockLength { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, Device, FhirBoolean>> SurgeonMachineRequirements { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> SurgicalOverheads { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> SurgeonScenarioMaximumNumberPatientMeans { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<INullableValue<int>, FhirDecimal>> ScenarioProbabilities { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>> SurgicalDurations { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> SurgeonScenarioMaximumNumberPatientStandardDeviations { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> DayAvailabilities { get; }

        /// <inheritdoc />
        public PositiveInt MaximumNumberRecoveryWardBeds { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, FhirDecimal>> SurgeonPenaltyWeights { get; }

        /// <inheritdoc />
        public ImmutableList<PositiveInt> Belien2007LengthOfStayDays { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> Belien2007ActivePeriods { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> Belien2007DayNumberTimeBlocks { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> Belien2007DayBedCapacities { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> Belien2007SurgeonStateProbabilities { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, PositiveInt>> Belien2007SurgeonLengthOfStayMaximums { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, PositiveInt>> Belien2007SurgeonNumberStates { get; }

        /// <inheritdoc />
        public FhirDecimal Belien2007MeanWeight { get; }

        /// <inheritdoc />
        public FhirDecimal Belien2007VarianceWeight { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> Ma2013ActiveDays { get; }

        /// <inheritdoc />
        public ImmutableList<PositiveInt> Ma2013BlockTypes { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups { get; }

        /// <inheritdoc />
        public ImmutableList<PositiveInt> Ma2013PatientGroups { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<PositiveInt, Duration>> Ma2013BlockTypeTimeBlockLengths { get; }

        /// <inheritdoc />
        public ImmutableList<Tuple<FhirDateTime, Location, Duration>> Ma2013DayOperatingRoomOperatingCapacities { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, PositiveInt>> Ma2013SurgeonGroupSubsetPatientGroups { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, PositiveInt>> Ma2013WardSubsetPatientGroups { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardα { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardβ { get; }

        /// <inheritdoc />
        public ImmutableList<KeyValuePair<Organization, FhirDecimal>> Ma2013Wardγ { get; }

        // Surgical Specialty 1 - General / Mixed (GEN / MIX)
        private Organization SurgicalSpecialty1GEN { get; }

        // Surgical Specialty 2 - Gynecology (GYN)
        private Organization SurgicalSpecialty2GYN { get; }

        // Surgical Specialty 3 - Plastic (PLA)
        private Organization SurgicalSpecialty3PLA { get; }

        // Surgical Specialty 4 - Ear-Nose-Throat (ENT)
        private Organization SurgicalSpecialty4ENT { get; }

        // Surgical Specialty 5 - Orthopedic (ORT)
        private Organization SurgicalSpecialty5ORT { get; }

        // Surgical Specialty 6 - Ophthalmology / Eye (EYE)
        private Organization SurgicalSpecialty6EYE { get; }

        // Surgical Specialty 7 - Urology (URO)
        private Organization SurgicalSpecialty7URO { get; }

        // Indices: d, d1, d2
        private ImmutableList<INullableValue<int>> GenerateWeekdays(
            INullableValueFactory nullableValueFactory,
            int numberWeekdaysPerWeek)
        {
            return Enumerable
                .Range(1, numberWeekdaysPerWeek)
                .Select(i => nullableValueFactory.Create<int>(i))
                .ToImmutableList();
        }

        // Index: j
        private ImmutableList<Tuple<Organization, ImmutableList<Organization>>> GenerateSurgicalSpecialties()
        {
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>>.Builder surgicalSpecialtiesBuilder = ImmutableList.CreateBuilder<Tuple<Organization, ImmutableList<Organization>>>();

            surgicalSpecialtiesBuilder.AddRange(
                new List<Tuple<Organization, ImmutableList<Organization>>>()
                {
                        Tuple.Create(
                            this.SurgicalSpecialty1GEN,
                            new List<Organization>()
                            {
                                this.GetSurgeonWithId(
                                    "1",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "2",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "3",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "4",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "5",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "6",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "7",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "8",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "9",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "10",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "11",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "12",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "13",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "14",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "15",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "16",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "17",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "18",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "19",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "20",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "21",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "22",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "23",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "24",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "25",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "26",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "27",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "28",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "29",
                                    this.Surgeons),
                                this.GetSurgeonWithId(
                                    "30",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty2GYN,
                        new List<Organization>()
                        {
                            this.GetSurgeonWithId(
                                    "31",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "32",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty3PLA,
                        new List<Organization>()
                        {
                           this.GetSurgeonWithId(
                                    "33",
                                    this.Surgeons),
                           this.GetSurgeonWithId(
                                    "34",
                                    this.Surgeons),
                           this.GetSurgeonWithId(
                                    "35",
                                    this.Surgeons),
                           this.GetSurgeonWithId(
                                    "36",
                                    this.Surgeons),
                           this.GetSurgeonWithId(
                                    "37",
                                    this.Surgeons),
                           this.GetSurgeonWithId(
                                    "38",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty4ENT,
                        new List<Organization>()
                        {
                            this.GetSurgeonWithId(
                                    "39",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "40",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "41",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "42",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "43",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "44",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "45",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty5ORT,
                        new List<Organization>()
                        {
                            this.GetSurgeonWithId(
                                    "46",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "47",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty6EYE,
                        new List<Organization>()
                        {
                           this.GetSurgeonWithId(
                                    "48",
                                    this.Surgeons)
                        }
                        .ToImmutableList()),

                    Tuple.Create(
                        this.SurgicalSpecialty7URO,
                        new List<Organization>()
                        {
                            this.GetSurgeonWithId(
                                    "49",
                                    this.Surgeons),
                            this.GetSurgeonWithId(
                                    "50",
                                    this.Surgeons)
                        }
                        .ToImmutableList())
                }
                .ToImmutableList());

            return surgicalSpecialtiesBuilder.ToImmutableList();
        }

        // Index: k
        private ImmutableList<INullableValue<int>> GenerateClusters(
            INullableValueFactory nullableValueFactory,
            int numberClusters)
        {
            return Enumerable
                .Range(1, numberClusters)
                .Select(i => nullableValueFactory.Create<int>(i))
                .ToImmutableList();
        }

        // Index: l, where L(s) is the maximum for surgical team s
        private ImmutableList<INullableValue<int>> GenerateLengthOfStayDays(
            INullableValueFactory nullableValueFactory,
            int maximumLengthOfStay)
        {
            return Enumerable
                .Range(0, maximumLengthOfStay + 1)
                .Select(i => nullableValueFactory.Create<int>(i))
                .ToImmutableList();
        }

        // Index: M
        private Bundle GenerateMachines(
            IBundleFactory bundleFactory,
            IDeviceFactory deviceFactory,
            IEntryComponentFactory entryComponentFactory,
            int numberMachines)
        {
            return bundleFactory.Create(
                Enumerable
                .Range(1, numberMachines)
                .Select(i =>
                entryComponentFactory.Create(
                    deviceFactory.Create(
                        i.ToString())))
                .ToList());
        }

        // Index: r
        private Bundle GenerateOperatingRooms(
            IBundleFactory bundleFactory,
            IEntryComponentFactory entryComponentFactory,
            ILocationFactory locationFactory,
            int numberOperatingRooms)
        {
            return bundleFactory.Create(
                Enumerable
                .Range(1, numberOperatingRooms)
                .Select(i =>
                entryComponentFactory.Create(
                    locationFactory.Create(
                        i.ToString())))
                .ToList());
        }

        // Index: s
        private Bundle GenerateSurgeons(
            IBundleFactory bundleFactory,
            IEntryComponentFactory entryComponentFactory,
            IOrganizationFactory organizationFactory,
            int numberSurgeons)
        {
            return bundleFactory.Create(
                Enumerable
                .Range(1, numberSurgeons)
                .Select(i =>
                entryComponentFactory.Create(
                    organizationFactory.Create(
                        i.ToString())))
                .ToList());
        }

        // Index: t
        private ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> GeneratePlanningHorizon(
            IFhirDateTimeFactory FhirDateTimeFactory,
            INullableValueFactory nullableValueFactory,
            DateTime endDate,
            DateTime startDate)
        {
            ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<PositiveInt, FhirDateTime>>();

            for (DateTime dt1 = startDate; dt1 <= endDate; dt1 = dt1.AddDays(1))
            {
                builder.Add(
                    KeyValuePair.Create(
                        (PositiveInt)nullableValueFactory.Create<int>(
                            (dt1 - startDate).Days + 1),
                        FhirDateTimeFactory.Create(
                            dt1.Date)));
            }

            return builder.ToImmutableList();
        }

        // Index: Λ
        private ImmutableList<INullableValue<int>> GenerateScenarios(
            INullableValueFactory nullableValueFactory,
            int numberScenarios)
        {
            return Enumerable
                .Range(1, numberScenarios)
                .Select(i => nullableValueFactory.Create<int>(i))
                .ToImmutableList();
        }

        // OperatingRoomServiceLevels
        // Index: υ1
        // Used in: 1A, 1B
        private ImmutableList<INullableValue<int>> GenerateOperatingRoomServiceLevels(
            INullableValueFactory nullableValueFactory,
            int numberOperatingRoomServiceLevels)
        {
            return Enumerable
                .Range(1, numberOperatingRoomServiceLevels)
                .Select(i => nullableValueFactory.Create<int>(
                    i))
                .ToImmutableList();
        }

        // Parameter: A(s, υ1)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>> GenerateSurgeonServiceLevelNumberTimeBlocks(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>>>.Builder surgeonServiceLevelNumberTimeBlocksBuilder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<int>>>();

            foreach (Organization surgeon in Surgeons.Entry.Select(x => (Organization)x.Resource))
            {
                surgeonServiceLevelNumberTimeBlocksBuilder.Add(
                        Tuple.Create(
                            surgeon,
                            this.OperatingRoomServiceLevels[0],
                            nullableValueFactory.Create<int>(
                                4)));

                surgeonServiceLevelNumberTimeBlocksBuilder.Add(
                        Tuple.Create(
                            surgeon,
                            this.OperatingRoomServiceLevels[1],
                            nullableValueFactory.Create<int>(
                                8)));

                surgeonServiceLevelNumberTimeBlocksBuilder.Add(
                        Tuple.Create(
                            surgeon,
                            this.OperatingRoomServiceLevels[2],
                            nullableValueFactory.Create<int>(
                                12)));
            }

            return surgeonServiceLevelNumberTimeBlocksBuilder.ToImmutableList();
        }

        // Parameter: C(m)
        // Used in: 3A
        private ImmutableList<KeyValuePair<Device, Money>> GenerateMachineCostsSameCost(
            IMoneyFactory moneyFactory,
            decimal cost,
            Money.Currencies currency)
        {
            return this.Machines
                .Entry
                .Where(i => i.Resource is Device)
                .Select(i => KeyValuePair.Create(
                    (Device)i.Resource,
                    moneyFactory.Create(
                        cost,
                        currency)))
                .ToImmutableList();
        }

        // Parameter: f(s, k)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> GenerateSurgicalFrequenciesVanHoudenhoven2007(
            ImmutableList<INullableValue<int>> clusters,
            Bundle surgeons,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>();

            VanHoudenhoven2007.InterfacesAbstractFactories.IAbstractFactory abstractFactory = VanHoudenhoven2007.AbstractFactories.AbstractFactory.Create();
            VanHoudenhoven2007.InterfacesAbstractFactories.IContextsAbstractFactory contextsAbstractFactory = abstractFactory.CreateContextsAbstractFactory();
            VanHoudenhoven2007.InterfacesAbstractFactories.IDependenciesAbstractFactory dependenciesAbstractFactory = abstractFactory.CreateDependenciesAbstractFactory();
            VanHoudenhoven2007.InterfacesAbstractFactories.IExportsAbstractFactory exportsAbstractFactory = abstractFactory.CreateExportsAbstractFactory();

            foreach (Tuple<Organization, ImmutableList<Organization>> item in surgicalSpecialties)
            {
                CodeableConcept specialty = item.Item1.Id switch
                {
                    "1" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGeneralSurgery(),

                    "2" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGynecologicalSurgery(),

                    "3" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreatePlasticSurgery(),

                    "4" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateEarNoseThroatSurgery(),

                    "5" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateOrthopedicSurgery(),

                    "6" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateOphthalmicSurgery(),

                    "7" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateUrology(),

                    _ => null
                };

                foreach (Organization surgeon in item.Item2)
                {
                    foreach (INullableValue<int> cluster in clusters)
                    {
                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalFrequencies.ISurgicalFrequencyInputContext surgicalFrequencyInputContext = contextsAbstractFactory.CreateSurgicalFrequencyInputContextFactory().Create(
                            category: cluster,
                            specialty: specialty);

                        VanHoudenhoven2007.Interfaces.Exports.SurgicalFrequencies.ISurgicalFrequencyExport surgicalFrequencyExport = exportsAbstractFactory.CreateSurgicalFrequencyExportFactory().Create();

                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalFrequencies.ISurgicalFrequencyOutputContext surgicalFrequencyOutputContext = surgicalFrequencyExport.GetSurgicalFrequency(
                            abstractFactory: abstractFactory,
                            surgicalFrequencyInputContext: surgicalFrequencyInputContext);

                        builder.Add(
                            Tuple.Create(
                                surgeon,
                                cluster,
                                surgicalFrequencyOutputContext.Frequency));
                    }
                }
            }

            return builder.ToImmutableList();
        }

        // Parameter: L(s)
        private ImmutableList<KeyValuePair<Organization, INullableValue<int>>> GenerateSurgeonLengthOfStayMaximumsSameForAllSurgeons(
            INullableValueFactory nullableValueFactory,
            int maximumLengthOfStay,
            Bundle surgeons)
        {
            ImmutableList<KeyValuePair<Organization, INullableValue<int>>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, INullableValue<int>>>();

            foreach (Organization surgeon in surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                builder.Add(
                    KeyValuePair.Create(
                        surgeon,
                        nullableValueFactory.Create<int>(
                            maximumLengthOfStay)));
            }

            return builder.ToImmutableList();
        }

        // Parameter: N(s)
        private ImmutableList<KeyValuePair<Organization, INullableValue<int>>> GenerateSurgeonStrategicTargetsSameForAllSurgeons(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<KeyValuePair<Organization, INullableValue<int>>>.Builder surgeonStrategicTargetsBuilder = ImmutableList.CreateBuilder<KeyValuePair<Organization, INullableValue<int>>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                surgeonStrategicTargetsBuilder.Add(
                    KeyValuePair.Create(
                        surgeon,
                        nullableValueFactory.Create<int>(
                            40)));
            }

            return surgeonStrategicTargetsBuilder.ToImmutableList();
        }

        // Parameter: P(υ1)
        private ImmutableList<KeyValuePair<INullableValue<int>, INullableValue<decimal>>> GenerateServiceLevelProbabilities(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<KeyValuePair<INullableValue<int>, INullableValue<decimal>>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<INullableValue<int>, INullableValue<decimal>>>();

            builder.Add(
                KeyValuePair.Create(
                    this.OperatingRoomServiceLevels[0],
                    nullableValueFactory.Create<decimal>(
                        0.75m)));

            builder.Add(
                KeyValuePair.Create(
                    this.OperatingRoomServiceLevels[1],
                    nullableValueFactory.Create<decimal>(
                        0.85m)));

            builder.Add(
                KeyValuePair.Create(
                    this.OperatingRoomServiceLevels[2],
                    nullableValueFactory.Create<decimal>(
                        0.95m)));

            return builder.ToImmutableList();
        }

        // Parameter: p(s, l, Λ)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>> GenerateSurgeonDayScenarioLengthOfStayProbabilitiesVanOostrum2011(
            INullableValueFactory nullableValueFactory,
            IDiscreteUniformFactory discreteUniformFactory,
            IpCalculation pCalculation,
            Bundle surgeons,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>>();

            VanOostrum2011.InterfacesAbstractFactories.IAbstractFactory abstractFactory = VanOostrum2011.AbstractFactories.AbstractFactory.Create();
            VanOostrum2011.InterfacesAbstractFactories.IContextsAbstractFactory contextsAbstractFactory = abstractFactory.CreateContextsAbstractFactory();
            VanOostrum2011.InterfacesAbstractFactories.IDependenciesAbstractFactory dependenciesAbstractFactory = abstractFactory.CreateDependenciesAbstractFactory();
            VanOostrum2011.InterfacesAbstractFactories.IExportsAbstractFactory exportsAbstractFactory = abstractFactory.CreateExportsAbstractFactory();

            foreach (Tuple<Organization, ImmutableList<Organization>> item in surgicalSpecialties)
            {
                CodeableConcept specialty = item.Item1.Id switch
                {
                    "1" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGeneralSurgery(),

                    "2" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGynecology(),

                    "3" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreatePlasticSurgery(),

                    "4" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateEarNoseThroatSurgery(),

                    "5" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateOrthopedicSurgery(),

                    "6" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateEyeSurgery(),

                    "7" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateUrology(),

                    _ => null
                };

                VanOostrum2011.Interfaces.Contexts.PatientLengthOfStays.IPatientLengthOfStayInputContext patientLengthOfStayInputContext = contextsAbstractFactory.CreatePatientLengthOfStayInputContextFactory().Create(
                    specialty: specialty,
                    statistic: dependenciesAbstractFactory.CreateValueFactory().CreateAverage());

                VanOostrum2011.Interfaces.Exports.PatientLengthOfStays.IPatientLengthOfStayExport patientLengthOfStayExport = exportsAbstractFactory.CreatePatientLengthOfStayExportFactory().Create();

                VanOostrum2011.Interfaces.Contexts.PatientLengthOfStays.IPatientLengthOfStayOutputContext patientLengthOfStayOutputContext = patientLengthOfStayExport.GetPatientLengthOfStay(
                    abstractFactory: abstractFactory,
                    patientLengthOfStayInputContext: patientLengthOfStayInputContext);

                foreach (Organization surgeon in item.Item2)
                {
                    builder.AddRange(
                        pCalculation.GenerateScenarios(
                            nullableValueFactory,
                            discreteUniformFactory,
                            this.LengthOfStayDays,
                            this.Scenarios,
                            surgeon,
                            this.SurgeonLengthOfStayMaximums,
                            (double)patientLengthOfStayOutputContext.Duration.Value.Value));
                }
            }

            return builder.ToImmutableList();
        }

        // Parameter: ψ(t)
        private ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> GenerateDayAvailabilitiesAllOperatingRoomsUnavailableOnWeekends(
            IFhirDateTimeFactory FhirDateTimeFactory,
            INullableValueFactory nullableValueFactory,
            DateTime endDate,
            DateTime startDate)
        {
            ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<FhirDateTime, FhirBoolean>>();

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    builder.Add(
                       KeyValuePair.Create(
                           FhirDateTimeFactory.Create(
                               dt),
                           (FhirBoolean)nullableValueFactory.Create<bool>(
                               true)));
                }
                else
                {
                    builder.Add(
                       KeyValuePair.Create(
                           FhirDateTimeFactory.Create(
                               dt),
                           (FhirBoolean)nullableValueFactory.Create<bool>(
                               false)));
                }
            }

            return builder.ToImmutableList();
        }

        // Parameter: ζ(s, m)
        private ImmutableList<Tuple<Organization, Device, FhirBoolean>> GenerateSurgeonMachineRequirements(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<Tuple<Organization, Device, FhirBoolean>>.Builder surgeonMachineRequirementsBuilder = ImmutableList.CreateBuilder<Tuple<Organization, Device, FhirBoolean>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(w => w.Resource is Organization).Select(w => (Organization)w.Resource))
            {
                foreach (Device machine in this.Machines.Entry.Where(w => w.Resource is Device).Select(w => (Device)w.Resource))
                {
                    if (int.Parse(machine.Id) % int.Parse(surgeon.Id) == 0)
                    {
                        surgeonMachineRequirementsBuilder.Add(
                            Tuple.Create(
                                surgeon,
                                machine,
                                (FhirBoolean)nullableValueFactory.Create<bool>(
                                    true)));
                    }
                    else
                    {
                        surgeonMachineRequirementsBuilder.Add(
                            Tuple.Create(
                                surgeon,
                                machine,
                                (FhirBoolean)nullableValueFactory.Create<bool>(
                                    false)));
                    }
                }
            }

            return surgeonMachineRequirementsBuilder.ToImmutableList();
        }

        // Parameter: θ(s, k)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> GenerateSurgicalOverheads(
            INullableValueFactory nullableValueFactory,
            IContinuousUniformFactory continuousUniformFactory,
            double lower,
            double upper)
        {
            IContinuousDistribution continuousUniform = continuousUniformFactory.Create(
                lower: lower,
                upper: upper);

            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                foreach (INullableValue<int> cluster in this.Clusters)
                {
                    builder.Add(
                        Tuple.Create(
                            surgeon,
                            cluster,
                            nullableValueFactory.Create<decimal>(
                                (decimal)continuousUniform.Sample())));
                }
            }

            return builder.ToImmutableList();
        }

        // Parameter: μ(s, Λ)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>> GenerateSurgeonScenarioMaximumNumberPatientMeans(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>.Builder surgeonScenarioMaximumNumberPatientMeansBuilder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<decimal>>>();

            this.SurgeonScenarioMaximumNumberPatients.ForEach(i =>
            surgeonScenarioMaximumNumberPatientMeansBuilder.Add(
                Tuple.Create(
                    i.Item1,
                    i.Item2,
                    nullableValueFactory.Create<decimal>(
                        (decimal)i.Item3.Value.Value))));

            return surgeonScenarioMaximumNumberPatientMeansBuilder.ToImmutableList();
        }

        // Parameter: Ρ(Λ)
        private ImmutableList<KeyValuePair<INullableValue<int>, FhirDecimal>> GenerateScenarioProbabilities(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<KeyValuePair<INullableValue<int>, FhirDecimal>>.Builder scenarioProbabilitiesBuilder = ImmutableList.CreateBuilder<KeyValuePair<INullableValue<int>, FhirDecimal>>();

            scenarioProbabilitiesBuilder.AddRange(
                new List<KeyValuePair<INullableValue<int>, FhirDecimal>>()
                {
                    KeyValuePair.Create(
                        this.Scenarios.Where(i => i.Value.Value == 1).SingleOrDefault(),
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            (decimal)40/(decimal)100)),

                    KeyValuePair.Create(
                        this.Scenarios.Where(i => i.Value.Value == 2).SingleOrDefault(),
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            (decimal)30/(decimal)100)),

                    KeyValuePair.Create(
                        this.Scenarios.Where(i => i.Value.Value == 3).SingleOrDefault(),
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            (decimal)20/(decimal)100)),

                    KeyValuePair.Create(
                        this.Scenarios.Where(i => i.Value.Value == 4).SingleOrDefault(),
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            (decimal)10/(decimal)100)),
                });

            return scenarioProbabilitiesBuilder.ToImmutableList();
        }

        // Parameter: ρ(s, k, Λ)
        private ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>> GenerateSurgicalDurationsVanHoudenhoven2007(
            IDurationFactory durationFactory,
            INullableValueFactory nullableValueFactory,
            ILogNormalFactory logNormalFactory,
            IρCalculation ρCalculation,
            ImmutableList<INullableValue<int>> clusters,
            ImmutableList<INullableValue<int>> scenarios,
            Bundle surgeons,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>>>();

            VanHoudenhoven2007.InterfacesAbstractFactories.IAbstractFactory abstractFactory = VanHoudenhoven2007.AbstractFactories.AbstractFactory.Create();

            VanHoudenhoven2007.InterfacesAbstractFactories.IContextsAbstractFactory contextsAbstractFactory = abstractFactory.CreateContextsAbstractFactory();
            VanHoudenhoven2007.InterfacesAbstractFactories.IDependenciesAbstractFactory dependenciesAbstractFactory = abstractFactory.CreateDependenciesAbstractFactory();
            VanHoudenhoven2007.InterfacesAbstractFactories.IExportsAbstractFactory exportsAbstractFactory = abstractFactory.CreateExportsAbstractFactory();

            foreach (Tuple<Organization, ImmutableList<Organization>> item in surgicalSpecialties)
            {
                CodeableConcept specialty = item.Item1.Id switch
                {
                    "1" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGeneralSurgery(),

                    "2" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateGynecologicalSurgery(),

                    "3" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreatePlasticSurgery(),

                    "4" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateEarNoseThroatSurgery(),

                    "5" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateOrthopedicSurgery(),

                    "6" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateOphthalmicSurgery(),

                    "7" => dependenciesAbstractFactory.CreateCodeableConceptFactory().CreateUrology(),

                    _ => null
                };

                foreach (Organization surgeon in item.Item2)
                {
                    foreach (PositiveInt cluster in clusters)
                    {
                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalDurations.ISurgicalDurationInputContext surgicalDurationAverageInputContext = contextsAbstractFactory.CreateSurgicalDurationInputContextFactory().Create(
                            category: cluster,
                            specialty: specialty,
                            statistic: dependenciesAbstractFactory.CreateValueFactory().CreateAverage());

                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalDurations.ISurgicalDurationInputContext surgicalDurationStdDevInputContext = contextsAbstractFactory.CreateSurgicalDurationInputContextFactory().Create(
                            category: cluster,
                            specialty: specialty,
                            statistic: dependenciesAbstractFactory.CreateValueFactory().CreateStdDev());

                        VanHoudenhoven2007.Interfaces.Exports.SurgicalDurations.ISurgicalDurationExport surgicalDurationAverageExport = exportsAbstractFactory.CreateSurgicalDurationExportFactory().Create();

                        VanHoudenhoven2007.Interfaces.Exports.SurgicalDurations.ISurgicalDurationExport surgicalDurationStdDevExport = exportsAbstractFactory.CreateSurgicalDurationExportFactory().Create();

                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalDurations.ISurgicalDurationOutputContext surgicalDurationAverageOutputContext = surgicalDurationAverageExport.GetSurgicalDuration(
                            abstractFactory: abstractFactory,
                            surgicalDurationInputContext: surgicalDurationAverageInputContext);

                        VanHoudenhoven2007.Interfaces.Contexts.SurgicalDurations.ISurgicalDurationOutputContext surgicalDurationStdDevOutputContext = surgicalDurationStdDevExport.GetSurgicalDuration(
                            abstractFactory: abstractFactory,
                            surgicalDurationInputContext: surgicalDurationStdDevInputContext);

                        builder.AddRange(
                            ρCalculation.CalculateLogNormal(
                                nullableValueFactory: nullableValueFactory,
                                logNormalFactory: logNormalFactory,
                                cluster: cluster,
                                scenarios: scenarios,
                                surgeon: surgeon,
                                µ: (double)surgicalDurationAverageOutputContext.Duration.ToHour(
                                    durationFactory).Value.Value,
                                σ: (double)surgicalDurationStdDevOutputContext.Duration.ToHour(
                                    durationFactory).Value.Value));
                    }
                }
            }

            return builder.ToImmutableList();
        }

        // Parameter: σ(s, Λ)
        private ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>> GenerateSurgeonScenarioMaximumNumberPatientStandardDeviations(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<Tuple<Organization, INullableValue<int>, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, INullableValue<int>, FhirDecimal>>();

            this.SurgeonScenarioMaximumNumberPatients.ForEach(i =>
            builder.Add(
                Tuple.Create(
                    i.Item1,
                    i.Item2,
                    (FhirDecimal)nullableValueFactory.Create<decimal>(
                        0m))));

            return builder.ToImmutableList();
        }

        // Parameter: ω(s)
        private ImmutableList<KeyValuePair<Organization, FhirDecimal>> GenerateSurgeonPenaltyWeightsSameForAllSurgeons(
            INullableValueFactory nullableValueFactory,
            Bundle surgeons,
            decimal penaltyWeight)
        {
            ImmutableList<KeyValuePair<Organization, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, FhirDecimal>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                builder.Add(
                    KeyValuePair.Create(
                        surgeon,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            penaltyWeight)));
            }

            return builder.ToImmutableList();
        }

        // Parameter: p(s, d)
        public ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> GetBelien2007SurgeonDayLengthOfStayProbabilities(
            PositiveInt scenario)
        {
            ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, PositiveInt, FhirDecimal>>();

            foreach (Tuple<Organization, INullableValue<int>, INullableValue<int>, INullableValue<decimal>> item in this.SurgeonDayScenarioLengthOfStayProbabilities.Where(w => w.Item3 == scenario))
            {
                builder.Add(
                    Tuple.Create(
                        item.Item1,
                        this.Belien2007LengthOfStayDays.Where(y => y.Value.Value == item.Item2.Value.Value + 1).SingleOrDefault(), // Shift up by 1; Day d in HM is day d+1 in Belien2007
                        (FhirDecimal)item.Item4));
            }

            return builder.ToImmutableList();
        }

        // Belien2007: wMean
        public FhirDecimal GetBelien2007MeanWeightSMIP1()
        {
            return (FhirDecimal)this.NullableValueFactory.Create<decimal>(
                1.0m);
        }

        // Belien2007: wVariance
        public FhirDecimal GetBelien2007VarianceWeightSMIP1()
        {
            return (FhirDecimal)this.NullableValueFactory.Create<decimal>(
                0.0m);
        }

        // Ma2013: dur(p)
        public ImmutableList<KeyValuePair<PositiveInt, Duration>> GetMa2013PatientGroupSurgeryDurations(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> surgicalDurations,
            ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> surgicalOverheads)
        {
            ImmutableList<KeyValuePair<PositiveInt, Duration>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<PositiveInt, Duration>>();

            foreach (ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>> item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item2))
            {
                foreach (Tuple<Organization, ImmutableList<PositiveInt>> surgeonGroupPatientGroups in item)
                {
                    Organization surgeon = surgeonGroupPatientGroups.Item1;

                    int min = surgeonGroupPatientGroups.Item2.Select(w => w.Value.Value).Min();

                    foreach (PositiveInt patientGroup in surgeonGroupPatientGroups.Item2)
                    {
                        int patientGroupValue = patientGroup.Value.Value;

                        INullableValue<int> cluster = this.Clusters.Where(w => w.Value.Value == patientGroupValue - min + 1).SingleOrDefault();

                        Duration duration = this.DurationFactory.CreateHour(
                            surgicalDurations.Where(w => w.Item1 == surgeon && w.Item2 == cluster && w.Item3 == scenario).Select(w => w.Item4.Value.Value).SingleOrDefault()
                            *
                            surgicalOverheads.Where(w => w.Item1 == surgeon && w.Item2 == cluster).Select(w => w.Item3.Value.Value).SingleOrDefault());

                        builder.Add(
                            KeyValuePair.Create(
                                patientGroup,
                                duration));
                    }
                }
            }

            return builder.ToImmutableList();
        }

        // Ma2013: THR(p)
        public ImmutableList<KeyValuePair<PositiveInt, PositiveInt>> GetMa2013PatientGroupThroughputsEvenlyDistributed(
            ImmutableList<KeyValuePair<Organization, PositiveInt>> HM1BSurgeonNumberAssignedTimeBlocks,
            ImmutableList<KeyValuePair<PositiveInt, Duration>> Ma2013PatientGroupSurgeryDurations,
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<KeyValuePair<Organization, PositiveInt>> surgeonStrategicTargets)
        {
            ImmutableList<KeyValuePair<Organization, int>>.Builder surgeonGroupNumberPatientsBuilder = ImmutableList.CreateBuilder<KeyValuePair<Organization, int>>();
            
            ImmutableList<KeyValuePair<PositiveInt, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<PositiveInt, PositiveInt>>();

            foreach (Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>> item0 in Ma2013WardSurgeonGroupPatientGroups)
            {
                Organization ward = item0.Item1;

                foreach (Tuple<Organization, ImmutableList<PositiveInt>> item1 in item0.Item2)
                {
                    Organization surgeonGroup = item1.Item1;

                    int surgeonGroupNumberBlocks = HM1BSurgeonNumberAssignedTimeBlocks.Where(w => w.Key == surgeonGroup).Select(w => w.Value.Value.Value).SingleOrDefault();

                    int numberPatients =
                        surgeonGroupNumberBlocks
                        *
                        this.SurgeonScenarioMaximumNumberPatients.Where(w => w.Item1 == surgeonGroup && w.Item2 == scenario).Select(w => w.Item3.Value.Value).SingleOrDefault();

                    surgeonGroupNumberPatientsBuilder.Add(
                        KeyValuePair.Create(
                            surgeonGroup,
                            numberPatients));
                }
            }

            ImmutableList<KeyValuePair<Organization, int>> surgeonGroupNumberPatients = surgeonGroupNumberPatientsBuilder.ToImmutableList();

            foreach (Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>> item0 in Ma2013WardSurgeonGroupPatientGroups)
            {
                Organization ward = item0.Item1;

                foreach (Tuple<Organization, ImmutableList<PositiveInt>> item1 in item0.Item2)
                {
                    Organization surgeonGroup = item1.Item1;

                    int min = item1.Item2.Select(w => w.Value.Value).Min();

                    foreach (PositiveInt patientGroup in item1.Item2)
                    {
                        int THR = 0;

                        decimal numberPatientGroups = 0;

                        int patientGroupValue = patientGroup.Value.Value;

                        INullableValue<int> cluster = this.Clusters.Where(w => w.Value.Value == patientGroupValue - min + 1).SingleOrDefault();

                        decimal durationAsDecimal = Ma2013PatientGroupSurgeryDurations.Where(w => w.Key == patientGroup).Select(w => w.Value.Value.Value).SingleOrDefault();

                        decimal frequency = this.SurgicalFrequencies.Where(w => w.Item1 == surgeonGroup && w.Item2 == cluster).Select(w => w.Item3.Value.Value).SingleOrDefault();

                        if (durationAsDecimal > 0m && durationAsDecimal <= this.TimeBlockLength.Value.Value && frequency > 0m)
                        {
                            int maximumNumberPatients = (int)Math.Floor((decimal)this.TimeBlockLength.Value.Value / (decimal)durationAsDecimal);

                            if (ward.Id == "1") // Gen: 30; 8 clusters; 2.7 days
                            {
                                numberPatientGroups = 8;
                            }
                            else if (ward.Id == "2") // Gyn: 2; 7 clusters; 2.3 days
                            {
                                numberPatientGroups = 7;
                            }
                            else if (ward.Id == "3") // Pla: 6; 7 clusters; 1.6 days
                            {
                                numberPatientGroups = 7;
                            }
                            else if (ward.Id == "4") // Ent: 7; 8 (7 less than 7.5h) clusters; 1.2 days
                            {
                                numberPatientGroups = 7;
                            }
                            else if (ward.Id == "5") // Ort: 2; 7 clusters; 2.2 days
                            {
                                numberPatientGroups = 7;
                            }
                            else if (ward.Id == "6") // Eye: 1; 5 clusters; 1.0 days
                            {
                                numberPatientGroups = 5;
                            }
                            else if (ward.Id == "7") // Uro: 2; 7 clusters; 3.4 days
                            {
                                numberPatientGroups = 7;
                            }

                            THR = (int)Math.Floor(
                                (double)surgeonGroupNumberPatients
                                .Where(w => w.Key == surgeonGroup)
                                .Select(w => w.Value)
                                .SingleOrDefault() 
                                / 
                                (double)numberPatientGroups);

                            builder.Add(
                                KeyValuePair.Create(
                                    patientGroup,
                                    (PositiveInt)this.NullableValueFactory.Create<int>(
                                        THR)));
                        }
                        else
                        {
                            builder.Add(
                                KeyValuePair.Create(
                                    patientGroup,
                                    (PositiveInt)this.NullableValueFactory.Create<int>(
                                        0)));
                        }
                    }
                }
            }

            foreach (Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>> item0 in Ma2013WardSurgeonGroupPatientGroups)
            {
                Organization ward = item0.Item1;

                foreach (Tuple<Organization, ImmutableList<PositiveInt>> item1 in item0.Item2)
                {
                    Organization surgeonGroup = item1.Item1;

                    int min = item1.Item2.Select(w => w.Value.Value).Min();

                    int numberAssignments = 0;

                    foreach (PositiveInt patientGroup in item1.Item2)
                    {
                        numberAssignments += builder.Where(w => w.Key == patientGroup).Select(w => w.Value.Value.Value).SingleOrDefault();
                    }

                    int i = 0;

                    do
                    {
                        if (i >= 8)
                        {
                            i = 0;
                        }

                        PositiveInt patientGroup = item1.Item2[i];

                        int patientGroupValue = patientGroup.Value.Value;

                        INullableValue<int> cluster = this.Clusters.Where(w => w.Value.Value == patientGroupValue - min + 1).SingleOrDefault();

                        decimal durationAsDecimal = Ma2013PatientGroupSurgeryDurations.Where(w => w.Key == patientGroup).Select(w => w.Value.Value.Value).SingleOrDefault();

                        decimal frequency = this.SurgicalFrequencies.Where(w => w.Item1 == surgeonGroup && w.Item2 == cluster).Select(w => w.Item3.Value.Value).SingleOrDefault();

                        if (
                            durationAsDecimal > 0m 
                            && 
                            durationAsDecimal <= this.TimeBlockLength.Value.Value 
                            && 
                            frequency > 0m
                            &&
                            numberAssignments + 1 <= surgeonGroupNumberPatients.Where(w => w.Key == surgeonGroup).Select(w => w.Value).SingleOrDefault())
                        {
                            int newValue = builder.Where(w => w.Key == patientGroup).Select(w => w.Value.Value.Value).SingleOrDefault() + 1;

                            KeyValuePair<PositiveInt, PositiveInt> itemToRemove = builder.Where(w => w.Key == patientGroup).SingleOrDefault();

                            builder.Remove(
                                itemToRemove);

                            builder.Add(
                                KeyValuePair.Create(
                                    patientGroup,
                                    (PositiveInt)this.NullableValueFactory.Create<int>(
                                        newValue)));
                        }
                            
                        i++;

                        numberAssignments++;
                    }
                    while (numberAssignments < surgeonGroupNumberPatients.Where(w => w.Key == surgeonGroup).Select(w => w.Value).SingleOrDefault());
                }
            }

            return builder.ToImmutableList();
        }

        // prob(p, l)
        public ImmutableList<Tuple<PositiveInt, PositiveInt, FhirDecimal>> GetMa2013PatientGroupDayLengthOfStayProbabilities(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups,
            PositiveInt scenario,
            ImmutableList<Tuple<Organization, PositiveInt, PositiveInt, FhirDecimal>> surgeonDayScenarioLengthOfStayProbabilities)
        {
            ImmutableList<Tuple<PositiveInt, PositiveInt, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<Tuple<PositiveInt, PositiveInt, FhirDecimal>>();

            foreach (ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>> item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item2))
            {
                foreach (Tuple<Organization, ImmutableList<PositiveInt>> surgeonGroupPatientGroups in item)
                {
                    Organization surgeon = surgeonGroupPatientGroups.Item1;

                    foreach (PositiveInt patientGroup in surgeonGroupPatientGroups.Item2)
                    {
                        foreach (PositiveInt day in this.LengthOfStayDays)
                        {
                            FhirDecimal probability = surgeonDayScenarioLengthOfStayProbabilities.Where(w => w.Item1 == surgeon && w.Item2 == day && w.Item3 == scenario).Select(w => w.Item4).SingleOrDefault();

                            builder.Add(
                                Tuple.Create(
                                    patientGroup,
                                    day,
                                    probability));
                        }
                    }
                }
            }

            return builder.ToImmutableList();
        }

        private Organization GetSurgeonWithId(
            string id,
            Bundle surgeons)
        {
            Organization surgeon = null;

            foreach (Organization item in surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                if (item.Id.Equals(id))
                {
                    surgeon = item;
                }
            }

            return surgeon;
        }

        private bool IsSurgeonMemberOfSurgicalSpecialty(
            Organization surgeon,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties,
            Organization surgicalSpecialty)
        {
            return surgicalSpecialties
                .Where(x => x.Item1 == surgicalSpecialty)
                .Select(x => x.Item2)
                .SingleOrDefault()
                .Contains(surgeon);
        }

        private Organization GetSurgicalSpecialtyOfSurgeon(
            Organization surgeon,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties)
        {
            Organization value = null;

            foreach (Tuple<Organization, ImmutableList<Organization>> item in surgicalSpecialties)
            {
                if (this.IsSurgeonMemberOfSurgicalSpecialty(
                    surgeon,
                    surgicalSpecialties,
                    item.Item1))
                {
                    value = item.Item1;
                }
            }

            return value;
        }

        // Belien2007: d, d1, d2 
        // Note: Index starts at 1 instead of 0
        private ImmutableList<PositiveInt> GenerateBelien2007LengthOfStayDays(
            INullableValueFactory nullableValueFactory,
            int Belien2007MaximumLengthOfStay)
        {
            return Enumerable
                .Range(1, Belien2007MaximumLengthOfStay)
                .Select(i => (PositiveInt)nullableValueFactory.Create<int>(i))
                .ToImmutableList();
        }

        // Belien2007: A
        private ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>> GenerateBelien2007ActivePeriodsAllOperatingRoomsUnavailableOnWeekends(
            DateTime endDate,
            DateTime startDate)
        {
            ImmutableList<KeyValuePair<FhirDateTime, FhirBoolean>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<FhirDateTime, FhirBoolean>>();

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    builder.Add(
                       KeyValuePair.Create(
                           this.FhirDateTimeFactory.Create(
                               dt),
                           (FhirBoolean)this.NullableValueFactory.Create<bool>(
                               false)));
                }
                else
                {
                    builder.Add(
                       KeyValuePair.Create(
                           this.FhirDateTimeFactory.Create(
                               dt),
                           (FhirBoolean)this.NullableValueFactory.Create<bool>(
                               true)));
                }
            }

            return builder.ToImmutableList();
        }

        // Belien2007: b(i)
        // Assumes that each operating room has one time block per active day
        private ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> GenerateBelien2007DayNumberTimeBlocks(
            INullableValueFactory nullableValueFactory,
            int numberOperatingRooms)
        {
            ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<FhirDateTime, PositiveInt>>();

            foreach (FhirDateTime day in this.Belien2007ActivePeriods.Where(i => i.Value.Value.Value).Select(i => i.Key))
            {
                builder.Add(
                    KeyValuePair.Create(
                        day,
                        (PositiveInt)nullableValueFactory.Create<int>(
                            numberOperatingRooms)));
            }

            return builder.ToImmutableList();
        }

        // Belien2007: c(i)
        // Assumes c(i) = MaximumNumberRecoveryWardBeds for each day i
        private ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>> GenerateBelien2007DayBedCapacities(
            Bundle surgeons)
        {
            ImmutableList<KeyValuePair<FhirDateTime, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<FhirDateTime, PositiveInt>>();

            foreach (FhirDateTime day in this.PlanningHorizon.Select(i => i.Value))
            {
                builder.Add(
                    KeyValuePair.Create(
                        day,
                        this.MaximumNumberRecoveryWardBeds));
            }

            return builder.ToImmutableList();
        }

        // Belien2007: m(s)
        private ImmutableList<KeyValuePair<Organization, PositiveInt>> GenerateBelien2007SurgeonLengthOfStayMaximumsSameForAllSurgeons(
            INullableValueFactory nullableValueFactory,
            int Belien2007MaximumLengthOfStay,
            Bundle surgeons)
        {
            ImmutableList<KeyValuePair<Organization, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, PositiveInt>>();

            foreach (Organization surgeon in surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                builder.Add(
                    KeyValuePair.Create(
                        surgeon,
                        (PositiveInt)nullableValueFactory.Create<int>(
                            Belien2007MaximumLengthOfStay)));
            }

            return builder.ToImmutableList();
        }

        // Belien2007: h(s, k)
        private ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>> GenerateBelien2007SurgeonStateProbabilities(
            Bundle surgeons)
        {
            ImmutableList<Tuple<Organization, PositiveInt, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, PositiveInt, FhirDecimal>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                foreach (PositiveInt scenario in this.Scenarios)
                {
                    builder.Add(
                        Tuple.Create(
                            surgeon,
                            scenario,
                            this.ScenarioProbabilities.Where(w => w.Key == scenario).Select(w => w.Value).SingleOrDefault()));
                }
            }

            return builder.ToImmutableList();
        }

        // Assumes q(s) = NumberScenarios for each surgeon s
        private ImmutableList<KeyValuePair<Organization, PositiveInt>> GenerateBelien2007SurgeonNumberStates(
            INullableValueFactory nullableValueFactory)
        {
            ImmutableList<KeyValuePair<Organization, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, PositiveInt>>();

            foreach (Organization surgeon in this.Surgeons.Entry.Where(i => i.Resource is Organization).Select(i => (Organization)i.Resource))
            {
                builder.Add(
                    KeyValuePair.Create(
                        surgeon,
                        (PositiveInt)this.NullableValueFactory.Create<int>(
                            this.Scenarios.Count())));
            }

            return builder.ToImmutableList();
        }

        // Ma2013: a
        private ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> GenerateMa2013ActiveDaysAllOperatingRoomsUnavailableOnWeekends(
            ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> planningHorizon)
        {
            ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<PositiveInt, FhirDateTime>>();

            foreach (KeyValuePair<PositiveInt, FhirDateTime> item in planningHorizon)
            {
                TimeSpan timeZone = TimeSpan.Zero;

                if (item.Value.ToDateTimeOffset(timeZone).UtcDateTime.DayOfWeek != DayOfWeek.Saturday && item.Value.ToDateTimeOffset(timeZone).UtcDateTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    builder.Add(
                        item);
                }
            }

            return builder.ToImmutableList();
        }

        // Ma2013: k
        private ImmutableList<PositiveInt> GenerateMa2013BlockTypesOnlyOneBlockType()
        {
            ImmutableList<PositiveInt>.Builder builder = ImmutableList.CreateBuilder<PositiveInt>();

            builder.Add(
                (PositiveInt)this.NullableValueFactory.Create<int>(
                    1));

            return builder.ToImmutableList();
        }

        // Ma2013: WardSurgeonGroupPatientGroups
        private ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> GenerateMa2013WardSurgeonGroupPatientGroups(
            INullableValueFactory nullableValueFactory,
            int numberClusters,
            ImmutableList<Tuple<Organization, ImmutableList<Organization>>> surgicalSpecialties)
        {
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>>.Builder builder = ImmutableList.CreateBuilder<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>>();

            int patientGroupCount = 1;

            foreach (Tuple<Organization, ImmutableList<Organization>> item in surgicalSpecialties)
            {
                ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>.Builder a = ImmutableList.CreateBuilder<Tuple<Organization, ImmutableList<PositiveInt>>>();

                foreach (Organization surgeon in item.Item2)
                {
                    ImmutableList<PositiveInt>.Builder b = ImmutableList.CreateBuilder<PositiveInt>();

                    b.AddRange(
                        Enumerable
                        .Range(patientGroupCount, numberClusters)
                        .Select(i => (PositiveInt)nullableValueFactory.Create<int>(i))
                        .ToImmutableList());

                    a.Add(
                        Tuple.Create(
                            surgeon,
                            b.ToImmutableList()));

                    patientGroupCount += numberClusters;
                }

                builder.Add(
                    Tuple.Create(
                        item.Item1,
                        a.ToImmutableList()));
            }

            return builder.ToImmutableList();
        }

        // Ma2013: p
        private ImmutableList<PositiveInt> GenerateMa2013PatientGroups(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {            
            return Ma2013WardSurgeonGroupPatientGroups
                .Select(a => a.Item2)
                .SelectMany(b => b.SelectMany(c => c.Item2))
                .ToImmutableList();
        }

        // Ma2013: Length(k)
        private ImmutableList<KeyValuePair<PositiveInt, Duration>> GenerateMa2013BlockTypeTimeBlockLengthsOnlyOneBlockType(
            ImmutableList<PositiveInt> Ma2013BlockTypes,
            Duration timeBlockLength)
        {
            ImmutableList<KeyValuePair<PositiveInt, Duration>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<PositiveInt, Duration>>();

            builder.Add(
                KeyValuePair.Create(
                    Ma2013BlockTypes.SingleOrDefault(),
                    timeBlockLength));

            return builder.ToImmutableList();
        }

        // Ma2013: ORday(a, r)
        private ImmutableList<Tuple<FhirDateTime, Location, Duration>> GenerateMa2013DayOperatingRoomOperatingCapacities(
            ImmutableList<KeyValuePair<PositiveInt, FhirDateTime>> Ma2013ActiveDays,
            Bundle operatingRooms,
            Duration timeBlockLength)
        {
            ImmutableList<Tuple<FhirDateTime, Location, Duration>>.Builder builder = ImmutableList.CreateBuilder<Tuple<FhirDateTime, Location, Duration>>();

            foreach (FhirDateTime activeDay in Ma2013ActiveDays.Select(w => w.Value))
            {
                foreach (Location operatingRoom in operatingRooms.Entry.Where(w => w.Resource is Location).Select(w => (Location)w.Resource))
                {
                    builder.Add(
                        Tuple.Create(
                            activeDay,
                            operatingRoom,
                            timeBlockLength));
                }
            }

            return builder.ToImmutableList();
        }

        // Ma2013: P(s)
        private ImmutableList<KeyValuePair<Organization, PositiveInt>> GenerateMa2013SurgeonGroupSubsetPatientGroups(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {
            ImmutableList<KeyValuePair<Organization, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, PositiveInt>>();

            foreach (ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>> item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item2))
            {
                foreach (Tuple<Organization, ImmutableList<PositiveInt>> surgeonGroupPatientGroups in item)
                {
                    foreach (PositiveInt patientGroup in surgeonGroupPatientGroups.Item2)
                    {
                        builder.Add(
                        KeyValuePair.Create(
                            surgeonGroupPatientGroups.Item1,
                            patientGroup));
                    }
                }
            }
            
            return builder.ToImmutableList();
        }

        // Ma2013: P(w)
        private ImmutableList<KeyValuePair<Organization, PositiveInt>> GenerateMa2013WardSubsetPatientGroups(
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {
            ImmutableList<KeyValuePair<Organization, PositiveInt>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, PositiveInt>>();

            foreach (Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>> wardSurgeonGroupPatientGroups in Ma2013WardSurgeonGroupPatientGroups)
            {
                foreach (Tuple<Organization, ImmutableList<PositiveInt>> surgeonGroupPatientGroups in wardSurgeonGroupPatientGroups.Item2)
                {
                    foreach (PositiveInt patientGroup in surgeonGroupPatientGroups.Item2)
                    {
                        builder.Add(
                        KeyValuePair.Create(
                            wardSurgeonGroupPatientGroups.Item1,
                            patientGroup));
                    }
                }
            }

            return builder.ToImmutableList();
        }

        // Ma2013: α(w)
        private ImmutableList<KeyValuePair<Organization, FhirDecimal>> GenerateMa2013Wardα(
            INullableValueFactory nullableValueFactory,
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {
            ImmutableList<KeyValuePair<Organization, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, FhirDecimal>>();

            foreach (Organization item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item1))
            {
                builder.Add(
                    KeyValuePair.Create(
                        item,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            0.333m)));
            }
            
            return builder.ToImmutableList();
        }

        // Ma2013: β(w)
        private ImmutableList<KeyValuePair<Organization, FhirDecimal>> GenerateMa2013Wardβ(
            INullableValueFactory nullableValueFactory,
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {
            ImmutableList<KeyValuePair<Organization, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, FhirDecimal>>();

            foreach (Organization item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item1))
            {
                builder.Add(
                    KeyValuePair.Create(
                        item,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            0.333m)));
            }

            return builder.ToImmutableList();
        }

        // Ma2013: γ(w)
        private ImmutableList<KeyValuePair<Organization, FhirDecimal>> GenerateMa2013Wardγ(
            INullableValueFactory nullableValueFactory,
            ImmutableList<Tuple<Organization, ImmutableList<Tuple<Organization, ImmutableList<PositiveInt>>>>> Ma2013WardSurgeonGroupPatientGroups)
        {
            ImmutableList<KeyValuePair<Organization, FhirDecimal>>.Builder builder = ImmutableList.CreateBuilder<KeyValuePair<Organization, FhirDecimal>>();

            foreach (Organization item in Ma2013WardSurgeonGroupPatientGroups.Select(w => w.Item1))
            {
                builder.Add(
                    KeyValuePair.Create(
                        item,
                        (FhirDecimal)nullableValueFactory.Create<decimal>(
                            0.333m)));
            }

            return builder.ToImmutableList();
        }
    }
}