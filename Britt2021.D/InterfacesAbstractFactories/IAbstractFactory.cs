namespace Britt2021.D.InterfacesAbstractFactories
{
    public interface IAbstractFactory
    {
        ICalculationsAbstractFactory CreateCalculationsAbstractFactory();

        IDependenciesAbstractFactory CreateDependenciesAbstractFactory();

        IExperimentsAbstractFactory CreateExperimentsAbstractFactory();
    }
}