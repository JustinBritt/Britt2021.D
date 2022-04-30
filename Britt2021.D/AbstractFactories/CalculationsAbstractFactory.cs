namespace Britt2021.D.AbstractFactories
{
    using Britt2021.D.Factories.Calculations;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Calculations;

    internal class CalculationsAbstractFactory : ICalculationsAbstractFactory
    {
        public CalculationsAbstractFactory()
        {
        }

        public IhCalculationFactory CreatehCalculationFactory()
        {
            IhCalculationFactory factory = null;

            try
            {
                factory = new hCalculationFactory();
            }
            finally
            {
            }

            return factory;
        }

        public InCalculationFactory CreatenCalculationFactory()
        {
            InCalculationFactory factory = null;

            try
            {
                factory = new nCalculationFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IpCalculationFactory CreatepCalculationFactory()
        {
            IpCalculationFactory factory = null;

            try
            {
                factory = new pCalculationFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IρCalculationFactory CreateρCalculationFactory()
        {
            IρCalculationFactory factory = null;

            try
            {
                factory = new ρCalculationFactory();
            }
            finally
            {
            }

            return factory;
        }
    }
}