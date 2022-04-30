namespace Britt2021.D.InterfacesAbstractFactories
{
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    public interface IDependenciesAbstractFactory
    {
        IBundleFactory CreateBundleFactory();

        IContinuousUniformFactory CreateContinuousUniformFactory();

        IDeviceFactory CreateDeviceFactory();

        IDiscreteUniformFactory CreateDiscreteUniformFactory();

        IDurationFactory CreateDurationFactory();

        IEntryComponentFactory CreateEntryComponentFactory();

        IFhirDateTimeFactory CreateFhirDateTimeFactory();

        ILocationFactory CreateLocationFactory();

        ILogNormalFactory CreateLogNormalFactory();

        IMoneyFactory CreateMoneyFactory();

        INullableValueFactory CreateNullableValueFactory();

        IOrganizationFactory CreateOrganizationFactory();
    }
}