namespace Britt2021.D.Factories.Comparers
{
    using Britt2021.D.Classes.Comparers;
    using Britt2021.D.Interfaces.Comparers;
    using Britt2021.D.InterfacesFactories.Comparers;

    internal sealed class FhirDateTimeComparerFactory : IFhirDateTimeComparerFactory
    {
        public FhirDateTimeComparerFactory()
        {
        }

        public IFhirDateTimeComparer Create()
        {
            IFhirDateTimeComparer instance = null;

            try
            {
                instance = new FhirDateTimeComparer();
            }
            finally
            {
            }

            return instance;
        }
    }
}