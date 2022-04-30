namespace Britt2021.D.AbstractFactories
{
    using Britt2021.D.Factories.Experiments;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Experiments;

    internal sealed class ExperimentsAbstractFactory : IExperimentsAbstractFactory
    {
        public ExperimentsAbstractFactory()
        {
        }

        public IExperiment1Factory CreateExperiment1Factory()
        {
            IExperiment1Factory factory = null;

            try
            {
                factory = new Experiment1Factory();
            }
            finally
            {
            }

            return factory;
        }
    }
}