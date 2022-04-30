namespace Britt2021.D.Factories.Experiments
{
    using Britt2021.D.Classes.Experiments;
    using Britt2021.D.Interfaces.Experiments;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Experiments;

    internal sealed class Experiment1Factory : IExperiment1Factory
    {
        public Experiment1Factory()
        {
        }

        public IExperiment1 Create(
            ICalculationsAbstractFactory calculationsAbstractFactory,
            IDependenciesAbstractFactory dependenciesAbstractFactory)
        {
            IExperiment1 experiment;

            try
            {
                experiment = new Experiment1(
                    calculationsAbstractFactory,
                    dependenciesAbstractFactory);
            }
            finally
            {
            }

            return experiment;
        }
    }
}