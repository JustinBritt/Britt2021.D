namespace Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model
{
    using global::Hl7.Fhir.Model;

    public interface IMoneyFactory
    {
        Money Create();

        Money Create(
            decimal value,
            Money.Currencies currency);
    }
}