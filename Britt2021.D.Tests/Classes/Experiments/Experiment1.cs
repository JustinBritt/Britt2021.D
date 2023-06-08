namespace Britt2021.D.Tests.Classes.Experiments
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Hl7.Fhir.Model;

    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.Interfaces.Experiments;

    [TestClass]
    public sealed class Experiment1
    {
        private IExperiment1 CreateExperiment1()
        {
            IAbstractFactory abstractFactory = Britt2021.D.AbstractFactories.AbstractFactory.Create();

            ICalculationsAbstractFactory calculationsAbstractFactory = abstractFactory.CreateCalculationsAbstractFactory();

            IDependenciesAbstractFactory dependenciesAbstractFactory = abstractFactory.CreateDependenciesAbstractFactory();

            IExperimentsAbstractFactory experimentsAbstractFactory = abstractFactory.CreateExperimentsAbstractFactory();

            return experimentsAbstractFactory.CreateExperiment1Factory().Create(
                calculationsAbstractFactory,
                dependenciesAbstractFactory);
        }

        [DataTestMethod]
        [DataRow(8)]
        public void NumberClusters(
            int numberClusters)
        {
            // Arrange
            IExperiment1 experiment1 = this.CreateExperiment1();

            // Act
            ImmutableList<INullableValue<int>> clusters = experiment1.Clusters;

            // Assert
            Assert.AreEqual(
                expected: numberClusters,
                actual: clusters.Count);
        }
    }
}