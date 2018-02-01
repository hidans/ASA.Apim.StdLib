using System;
using System.Collections.Generic;
using System.Linq;
using ASA.Apim.Library.CourseHeader_Service;
using ASA.Apim.Library.Security;

namespace ASA.Apim.Library
{
    public static class CourseService
    {
        #region CourseHeaders
        /// <summary>
        /// Henter detaljeret kurser fra navision.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeaders(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = new CourseHeader_Filter[]
            {
                new CourseHeader_Filter()
                {
                    Field = CourseHeader_Fields.Course_Type_No,
                    Criteria = filter
                }
            };

            return GetCourseHeaders(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }
        /// <summary>
        /// Henter detaljeret kurser fra navision. Kald kun denne metode hvis du selv opretter filter udfra CourseHeader_Filter og sender med.
        /// </summary>
        /// <param name="filter">Et object du selv opretter udfra CourseHeader_Filter.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeaders(CourseHeader_Filter[] filter, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };

            return GetCourseHeaders(filter, apiKeys, size);
        }
        internal static IEnumerable<CourseHeader> GetCourseHeaders(CourseHeader_Filter[] filter, ApiManagerCredentials credentials, int size)
        {
            try
            {
                var service = new ASA.Apim.Library.CourseHeader_Service.CourseHeader_Service()
                {
                    ApiCredentials = credentials
                };

                var list = service.ReadMultiple(filter, "", size).ToList();
                service.Dispose();
                return list;
            }
            catch(Exception exception)
            {
                Services.LogService.Error("Fejl i forespørgelse af kurser.", exception);
                return new List<CourseHeader>();
            }          
        }
        #endregion
    }
}