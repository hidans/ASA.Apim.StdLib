using ASA.Apim.Library.CourseHeader_Service;
using ASA.Apim.Library.CoursePrices_Service;
using ASA.Apim.Library.Entity;
using ASA.Apim.Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASA.Apim.Library
{
    public static class ApiService
    {
        /// <summary>
        /// Henter kurser med detaljeret info og pris fra navision. Udfra kursus typens gruppe id/nummer
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPricesByTypeGroupId(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = CourseService.SingleCourseHeaderFilter(filter, CourseHeader_Fields.Course_Type_Group_Code);
            return GetCourseHeadersWithPrices(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter kurser med detaljeret info og pris fra navision. Udfra kursus typens id/nummer
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPricesByTypeId(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = CourseService.SingleCourseHeaderFilter(filter, CourseHeader_Fields.Course_Type_No);
            return GetCourseHeadersWithPrices(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter kurser med detaljeret info og pris fra navision. Udfra kursus nummer/id.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPricesById(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = CourseService.SingleCourseHeaderFilter(filter, CourseHeader_Fields.No);
            return GetCourseHeadersWithPrices(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter kurser med detaljeret info og pris fra navision. Som du har adgang til via din subscriptionkey og accountkey.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPrices(string subscriptionKey, string accountKey = null, int size = 0)
        {
            return GetCourseHeadersWithPricesById(subscriptionKey, accountKey, "");
        }

        /// <summary>
        /// Henter kurser med detaljeret info og pris fra navision.Kald kun denne metode hvis du selv opretter filter udfra CourseHeader_Filter og sender med.
        /// </summary>
        /// <param name="filter">Et object du selv opretter udfra CourseHeader_Filter.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPrices(CourseHeader_Filter[] filter, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };

            return GetCourseHeadersWithPrices(filter, apiKeys, size);
        }
        internal static IEnumerable<CourseHeaderWithPrice> GetCourseHeadersWithPrices(CourseHeader_Filter[] filter, ApiManagerCredentials credentials, int size)
        {
            var list = new List<CourseHeaderWithPrice>();
            try
            {
                var courseHeaders = CourseService.GetCourseHeaders(filter, credentials, size);
                if (courseHeaders != null && courseHeaders.Any())
                {
                    List<CoursePrices> coursePrices = null;

                    var courseTypes = courseHeaders.Select(x => x.Course_Type_No).Distinct();
                    if (courseTypes != null && courseTypes.Any())
                    {
                        var criteria = string.Join("|", courseTypes);
                        if (!string.IsNullOrEmpty(criteria))
                        {
                            if(criteria.StartsWith("|"))
                            {
                                criteria = criteria.Substring(1);
                            }
                            if(criteria.EndsWith("|"))
                            {
                                criteria = criteria.Remove(criteria.Length - 1);
                            }
                            coursePrices = CourseService.GetCoursePricesByType(credentials.SubscriptionKey, credentials.AccountKey, criteria).ToList();
                        }
                    }

                    if (coursePrices == null)
                    {
                        coursePrices = new List<CoursePrices>();
                    }

                    foreach (var couseHeader in courseHeaders.Where(x => x != null).ToList())
                    {
                        var prices = coursePrices.Any() ? coursePrices.Where(x => x.CourseType.Equals(couseHeader.Course_Type_No)) : new List<CoursePrices>();
                        list.Add(new CourseHeaderWithPrice(couseHeader, prices));
                    }
                }
            }
            catch (Exception exception)
            {
                Services.LogService.Error("Fejl i sammensætning af kursus og priser.", exception);
            } 
                    
            return list;
        }
    }
}