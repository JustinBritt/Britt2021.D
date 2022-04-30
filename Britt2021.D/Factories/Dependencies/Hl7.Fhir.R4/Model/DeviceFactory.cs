namespace Britt2021.D.Factories.Dependencies.Hl7.Fhir.R4.Model
{
    using global::Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    internal sealed class DeviceFactory : IDeviceFactory
    {
        public DeviceFactory()
        {
        }

        public Device Create()
        {
            Device device;

            try
            {
                device = new Device();
            }
            finally
            {
            }

            return device;
        }

        public Device Create(
            string id)
        {
            Device device;

            try
            {
                device = new Device()
                {
                    Id = id
                };
            }
            finally
            {
            }

            return device;
        }
    }
}