﻿namespace Britt2021.D.InterfacesAbstractFactories
{
    using Britt2021.D.InterfacesFactories.Comparers;

    public interface IComparersAbstractFactory
    {
        IDeviceComparerFactory CreateDeviceComparerFactory();

        INullableValueintComparerFactory CreateNullableValueintComparerFactory();

        IOrganizationComparerFactory CreateOrganizationComparerFactory();
    }
}