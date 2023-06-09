namespace Britt2021.D.AbstractFactories
{
    using Britt2021.D.Factories.Comparers;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Comparers;

    internal sealed class ComparersAbstractFactory : IComparersAbstractFactory
    {
        public ComparersAbstractFactory()
        {
        }

        public INullableValueintComparerFactory CreateNullableValueintComparerFactory()
        {
            INullableValueintComparerFactory factory = null;

            try
            {
                factory = new NullableValueintComparerFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IOrganizationComparerFactory CreateOrganizationComparerFactory()
        {
            IOrganizationComparerFactory factory = null;

            try
            {
                factory = new OrganizationComparerFactory();
            }
            finally
            {
            }

            return factory;
        }
    }
}