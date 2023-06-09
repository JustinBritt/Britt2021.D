namespace Britt2021.D.InterfacesFactories.Comparers
{
    using Britt2021.D.Interfaces.Comparers;

    public interface IFhirDateTimeComparerFactory
    {
        IFhirDateTimeComparer Create();
    }
}