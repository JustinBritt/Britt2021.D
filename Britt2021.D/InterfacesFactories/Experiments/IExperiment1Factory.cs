namespace Britt2021.D.InterfacesFactories.Experiments
{
    using Britt2021.D.Interfaces.Experiments;
    using Britt2021.D.InterfacesAbstractFactories;

    public interface IExperiment1Factory
    {
        IExperiment1 Create(
            ICalculationsAbstractFactory calculationsAbstractFactory,
            IDependenciesAbstractFactory dependenciesAbstractFactory);
    }
}