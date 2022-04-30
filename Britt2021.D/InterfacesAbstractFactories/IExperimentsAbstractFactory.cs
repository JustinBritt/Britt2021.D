namespace Britt2021.D.InterfacesAbstractFactories
{
    using Britt2021.D.InterfacesFactories.Experiments;

    public interface IExperimentsAbstractFactory
    {
        IExperiment1Factory CreateExperiment1Factory();
    }
}