namespace Britt2021.D.Factories.Calculations
{
    using Britt2021.D.Classes.Calculations;
    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Calculations;

    internal sealed class hCalculationFactory : IhCalculationFactory
    {
        public hCalculationFactory()
        {
        }

        public IhCalculation Create()
        {
            IhCalculation calculation;

            try
            {
                calculation = new hCalculation();
            }
            finally
            {
            }

            return calculation;
        }
    }
}