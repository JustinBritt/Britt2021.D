namespace Britt2021.D.Factories.Dependencies.Hl7.Fhir.R4.Model
{
    using global::Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    internal sealed class MoneyFactory : IMoneyFactory
    {
        public MoneyFactory()
        {
        }

        public Money Create()
        {
            Money money;

            try
            {
                money = new Money();
            }
            finally
            {
            }

            return money;
        }

        public Money Create(
            decimal value,
            Money.Currencies currency)
        {
            Money money;

            try
            {
                money = new Money()
                {
                    Value = value,
                    Currency = currency
                };
            }
            finally
            {
            }

            return money;
        }
    }
}