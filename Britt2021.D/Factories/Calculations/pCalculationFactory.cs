namespace Britt2021.D.Factories.Calculations
{
    using Britt2021.D.Classes.Calculations;
    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Calculations;

    internal sealed class pCalculationFactory : IpCalculationFactory
    {
        public pCalculationFactory()
        {
        }

        public IpCalculation Create()
        {
            IpCalculation calculation;

            try
            {
                calculation = new pCalculation();
            }
            finally
            {
            }

            return calculation;
        }
    }
}