﻿namespace Britt2021.D.Factories.Comparers
{
    using Britt2021.D.Classes.Comparers;
    using Britt2021.D.Interfaces.Comparers;
    using Britt2021.D.InterfacesFactories.Comparers;

    internal sealed class OrganizationComparerFactory : IOrganizationComparerFactory
    {
        public OrganizationComparerFactory()
        {
        }

        public IOrganizationComparer Create()
        {
            IOrganizationComparer instance = null;

            try
            {
                instance = new OrganizationComparer();
            }
            finally
            {
            }

            return instance;
        }
    }
}