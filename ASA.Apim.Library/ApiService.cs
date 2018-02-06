using ASA.Apim.Library.Countries_Service;
using ASA.Apim.Library.Security;
using System;
using System.Collections.Generic;

namespace ASA.Apim.Library
{
    public static class ApiService
    {
        #region Country
        /// <summary>
        /// Henter lande fra navision. Udfra landenavn.
        /// </summary>
        /// <param name="countryName">Navnet på landet.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<Countries> GetCountriesByName(string countryName, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var filter = SingleCountriesFilter(countryName, Countries_Fields.Name);
            return GetCountries(filter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter lande fra navision. Udfra landekode.
        /// </summary>
        /// <param name="countryCode">Koden på landet.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<Countries> GetCountriesByCode(string countryCode, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var filter = SingleCountriesFilter(countryCode, Countries_Fields.Code);
            return GetCountries(filter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter alle lande tilgængelige fra navision.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<Countries> GetCountries(string subscriptionKey, string accountKey = null, int size = 0)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                SubscriptionKey = subscriptionKey,
                AccountKey = accountKey
            };
            return GetCountriesByName("", subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter lande fra navision. Kald kun denne metode hvis du selv opretter filter udfra Countries_Filter og sender med.
        /// </summary>
        /// <param name="filter">Et object du selv opretter udfra Countries_Filter.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<Countries> GetCountries(Countries_Filter[] filter, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                SubscriptionKey = subscriptionKey,
                AccountKey = accountKey
            };
            return GetCountries(filter, apiKeys, size);
        }
        internal static IEnumerable<Countries> GetCountries(Countries_Filter[] filter, ApiManagerCredentials credentials, int size = 0)
        {
            try
            {
                var service = new Countries_Service.Countries_Service()
                {
                    ApiCredentials = credentials
                };

                var list = service.ReadMultiple(filter, "", size);
                service.Dispose();
                return list;
            }
            catch(Exception exception)
            {
                Services.LogService.Error("Fejl i forsøg på at hente lande fra navision.", exception);
                return new List<Countries>();
            }
        }

        internal static Countries_Filter[] SingleCountriesFilter(string criteria, Countries_Fields field)
        {
            return new Countries_Filter[]
            {
                new Countries_Filter()
                { 
                    Field = field,
                    Criteria = criteria
                }
            };
        }
        #endregion
    }
}