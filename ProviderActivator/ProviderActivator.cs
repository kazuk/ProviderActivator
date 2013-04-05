using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderActivators
{
    public class ProviderActivator
    {
        public static ProviderBase InstanciateProvider(ProviderSettings providerSettings, Type providerType)
        {
            var typeNameConverter = new TypeNameConverter();

            var instanceType = (Type)typeNameConverter.ConvertFrom(providerSettings.Type);
            if (instanceType != null)
            {
                if (!providerType.IsAssignableFrom(instanceType))
                {
                    throw new ConfigurationErrorsException( instanceType +" is not "+ providerType );
                }
                var constructorInfo = instanceType.GetConstructor(Type.EmptyTypes);
                if (constructorInfo != null)
                {
                    var provider = (ProviderBase)constructorInfo.Invoke(null);
                    provider.Initialize(providerSettings.Name, providerSettings.Parameters);
                    return provider;
                }
            }
            return null;
        }

        public static TProvider InstanciateProvider<TProvider>(ProviderSettings providerSettings)
            where TProvider : ProviderBase
        {
            return  (TProvider)InstanciateProvider(providerSettings, typeof(TProvider) );
        }

        public static void InstanciateProviders(
            ProviderSettingsCollection providerSettings,
            ProviderCollection providerCollection,
            Type providerType)
        {
            providerCollection.AddRange(
                providerSettings.Cast<ProviderSettings>().Select(s => InstanciateProvider(s, providerType)));
        }

        public static void InstanciateProviders<TProvider>(
                ProviderSettingsCollection providerSettings,
                ProviderCollection providerCollection
            ) 
            where TProvider : ProviderBase
        {
            providerCollection.AddRange(
                providerSettings.Cast<ProviderSettings>().Select(s=>InstanciateProvider(s,typeof(TProvider)))
                );
        }
    }

    public static class ProviderCollectionExtention
    {
        public static void AddRange(this ProviderCollection providerCollection, IEnumerable<ProviderBase> providers)
        {
            foreach (var providerBase in providers)
            {
                providerCollection.Add(providerBase);
            }
        }
    }


}
