namespace Britt2021.D.InterfacesAbstractFactories
{
    using Britt2021.D.InterfacesFactories.Calculations;

    public interface ICalculationsAbstractFactory
    {
        IhCalculationFactory CreatehCalculationFactory();

        InCalculationFactory CreatenCalculationFactory();

        IpCalculationFactory CreatepCalculationFactory();

        IρCalculationFactory CreateρCalculationFactory();
    }
}