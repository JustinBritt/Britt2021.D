namespace Britt2021.D.Factories.Comparers
{
    using Britt2021.D.Classes.Comparers;
    using Britt2021.D.Interfaces.Comparers;
    using Britt2021.D.InterfacesFactories.Comparers;

    internal sealed class DeviceComparerFactory : IDeviceComparerFactory
    {
        public DeviceComparerFactory()
        {
        }

        public IDeviceComparer Create()
        {
            IDeviceComparer instance = null;

            try
            {
                instance = new DeviceComparer();
            }
            finally
            {
            }

            return instance;
        }
    }
}