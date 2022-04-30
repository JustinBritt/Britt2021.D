namespace Britt2021.D.AbstractFactories
{
    using Britt2021.D.Factories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.Factories.Dependencies.MathNet.Numerics.Distributions;
    using Britt2021.D.InterfacesAbstractFactories;
    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;
    using Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions;

    internal class DependenciesAbstractFactory : IDependenciesAbstractFactory
    {
        public DependenciesAbstractFactory()
        {
        }

        public IBundleFactory CreateBundleFactory()
        {
            IBundleFactory factory = null;

            try
            {
                factory = new BundleFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IContinuousUniformFactory CreateContinuousUniformFactory()
        {
            IContinuousUniformFactory factory = null;

            try
            {
                factory = new ContinuousUniformFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IDeviceFactory CreateDeviceFactory()
        {
            IDeviceFactory factory = null;

            try
            {
                factory = new DeviceFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IDiscreteUniformFactory CreateDiscreteUniformFactory()
        {
            IDiscreteUniformFactory factory = null;

            try
            {
                factory = new DiscreteUniformFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IDurationFactory CreateDurationFactory()
        {
            IDurationFactory factory = null;

            try
            {
                factory = new DurationFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IEntryComponentFactory CreateEntryComponentFactory()
        {
            IEntryComponentFactory factory = null;

            try
            {
                factory = new EntryComponentFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IFhirDateTimeFactory CreateFhirDateTimeFactory()
        {
            IFhirDateTimeFactory factory = null;

            try
            {
                factory = new FhirDateTimeFactory();
            }
            finally
            {
            }

            return factory;
        }

        public ILocationFactory CreateLocationFactory()
        {
            ILocationFactory factory = null;

            try
            {
                factory = new LocationFactory();
            }
            finally
            {
            }

            return factory;
        }

        public ILogNormalFactory CreateLogNormalFactory()
        {
            ILogNormalFactory factory = null;

            try
            {
                factory = new LogNormalFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IMoneyFactory CreateMoneyFactory()
        {
            IMoneyFactory factory = null;

            try
            {
                factory = new MoneyFactory();
            }
            finally
            {
            }

            return factory;
        }

        public INullableValueFactory CreateNullableValueFactory()
        {
            INullableValueFactory factory = null;

            try
            {
                factory = new NullableValueFactory();
            }
            finally
            {
            }

            return factory;
        }

        public IOrganizationFactory CreateOrganizationFactory()
        {
            IOrganizationFactory factory = null;

            try
            {
                factory = new OrganizationFactory();
            }
            finally
            {
            }

            return factory;
        }
    }
}