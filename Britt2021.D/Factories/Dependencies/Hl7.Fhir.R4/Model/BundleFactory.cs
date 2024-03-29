﻿namespace Britt2021.D.Factories.Dependencies.Hl7.Fhir.R4.Model
{
    using System.Collections.Generic;

    using global::Hl7.Fhir.Model;

    using Britt2021.D.InterfacesFactories.Dependencies.Hl7.Fhir.R4.Model;

    internal sealed class BundleFactory : IBundleFactory
    {
        public BundleFactory()
        {
        }

        public Bundle Create()
        {
            Bundle bundle;

            try
            {
                bundle = new Bundle();
            }
            finally
            {
            }

            return bundle;
        }

        public Bundle Create(
            List<Bundle.EntryComponent> entry)
        {
            Bundle bundle;

            try
            {
                bundle = new Bundle()
                {
                    Entry = entry
                };
            }
            finally
            {
            }

            return bundle;
        }
    }
}