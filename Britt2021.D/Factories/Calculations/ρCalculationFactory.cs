namespace Britt2021.D.Factories.Calculations
{
    using Britt2021.D.Classes.Calculations;
    using Britt2021.D.Interfaces.Calculations;
    using Britt2021.D.InterfacesFactories.Calculations;

    internal sealed class ρCalculationFactory : IρCalculationFactory
    {
        public ρCalculationFactory()
        {
        }

        public IρCalculation Create()
        {
            IρCalculation calculation;

            try
            {
                calculation = new ρCalculation();
            }
            finally
            {
            }

            return calculation;
        }
    }
}