namespace Britt2021.D.Factories.Calculations
{
    using Britt2021.D.Classes.Calculations;
    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Calculations;

    internal sealed class nCalculationFactory : InCalculationFactory
    {
        public nCalculationFactory()
        {
        }

        public InCalculation Create()
        {
            InCalculation calculation;

            try
            {
                calculation = new nCalculation();
            }
            finally
            {
            }

            return calculation;
        }
    }
}