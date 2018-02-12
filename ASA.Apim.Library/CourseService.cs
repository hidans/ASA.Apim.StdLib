using System;
using System.Collections.Generic;
using System.Linq;
using ASA.Apim.Library.CourseHeader_Service;
using ASA.Apim.Library.CoursePrices_Service;
using ASA.Apim.Library.Security;
using ASA.Apim.Library.CourseEnrollments_Service;
using ASA.Apim.Library.Entity;

namespace ASA.Apim.Library
{
    public static class CourseService
    {
        #region CourseHeader
        /// <summary>
        /// Henter et kursus med detaljeret info fra navision. Udfra kursus id/nummer
        /// </summary>
        /// <param name="courseNumber">Kursus id/nummer</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <returns></returns>
        public static CourseHeader GetCourseHeader(string courseNumber, string subscriptionKey, string accountKey = null)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };

            return GetCourseHeader(courseNumber, apiKeys);
        }
        internal static CourseHeader GetCourseHeader(string courseNumber, ApiManagerCredentials credentials)
        {
            try
            {
                var service = new ASA.Apim.Library.CourseHeader_Service.CourseHeader_Service()
                {
                    ApiCredentials = credentials
                };

                var courseHeader = service.Read(courseNumber);
                service.Dispose();
                return courseHeader;
            }
            catch (Exception exception)
            {
                Services.LogService.Error("Fejl i forespørgelse af kursus.", exception);
                return null;
            }
        }
        #endregion

        #region CourseHeaders
        /// <summary>
        /// Henter kurser med detaljeret info fra navision. Udfra kursus typens gruppe id/nummer
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeadersByTypeGroupId(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = SingleCourseHeaderFilter(filter, CourseHeader_Fields.Course_Type_Group_Code);
            return GetCourseHeaders(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter kurser med detaljeret info fra navision. Udfra kursus typens id/nummer
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeadersByTypeId(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = SingleCourseHeaderFilter(filter, CourseHeader_Fields.Course_Type_No);
            return GetCourseHeaders(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter kurser med detaljeret info fra navision. Udfra kursus nummer/id.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeadersById(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CourseHeaderfilter = SingleCourseHeaderFilter(filter, CourseHeader_Fields.No);
            return GetCourseHeaders(CourseHeaderfilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        /// Henter alle kurser med detaljeret info fra navision. Som du har adgang til via din subscriptionkey og accountkey.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CourseHeader> GetCourseHeaders(string subscriptionKey, string accountKey = null, int size = 0)
        {
            return GetCourseHeadersById(subscriptionKey, accountKey, "");
        }
        /// <summary>
        /// Henter kurser med detaljeret info fra navision. Kald kun denne metode hvis du selv opretter filter udfra CourseHeader_Filter og sender med.
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
                throw exception;
                return new List<CourseHeader>();
            }          
        }

        internal static CourseHeader_Filter[] SingleCourseHeaderFilter(string criteria, CourseHeader_Fields field)
        {
            return new CourseHeader_Filter[]
            {
                new CourseHeader_Filter()
                {
                    Field = field,
                    Criteria = criteria
                }
            };
        }
        #endregion

        #region CoursePrices

        /// <summary>
        /// Henter priser på kursus fra navision. Udfra kursustypen.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="filter">Søg på alt der Feks. starter med 10 -> "10.." eller omvendt, der slutter på 10 -> "..10" eller søg udfra id'er -> "1014" eller flere -> "1014|1015|1016" [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CoursePrices> GetCoursePricesByType(string subscriptionKey, string accountKey = null, string filter = "", int size = 0)
        {
            var CoursePricefilter = SingleCoursePriceFilter(filter, CoursePrices_Fields.CourseType);
            return GetCoursePrices(CoursePricefilter, subscriptionKey, accountKey, size);
        }

        /// <summary>
        ///  Henter alle priser for kursustyper fra navision. Som du har adgang til via din subscriptionkey og accountkey.
        /// </summary>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CoursePrices> GetCoursePrices(string subscriptionKey, string accountKey = null, int size = 0)
        {
            return GetCoursePricesByType(subscriptionKey, accountKey, "");
        }

        /// <summary>
        /// Henter alle priser for alle kursustyper fra navision. Kald kun denne metode hvis du selv opretter filter udfra CoursePrices_Filter og sender med.
        /// </summary>
        /// <param name="filter">Et object du selv opretter udfra CoursePrices_Filter.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <param name="size">Maks. indhold i listen som kommer return. 0 er ubegrænset. [Optional]</param>
        /// <returns></returns>
        public static IEnumerable<CoursePrices> GetCoursePrices(CoursePrices_Filter[] filter, string subscriptionKey, string accountKey = null, int size = 0)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };

            return GetCoursePrices(filter, apiKeys, size);
        }
        internal static IEnumerable<CoursePrices> GetCoursePrices(CoursePrices_Filter[] filter, ApiManagerCredentials credentials, int size)
        {
            try
            {
                var service = new ASA.Apim.Library.CoursePrices_Service.CoursePrices_Service()
                {
                    ApiCredentials = credentials
                };

                var list = service.ReadMultiple(filter, "", size).ToList();
                service.Dispose();
                return list;
            }
            catch (Exception exception)
            {
                Services.LogService.Error("Fejl i forespørgelse af kursus priser.", exception);
                throw exception;
                return new List<CoursePrices>();
            }
        }

        internal static CoursePrices_Filter[] SingleCoursePriceFilter(string criteria, CoursePrices_Fields field)
        {
            return new CoursePrices_Filter[]
            {
                new CoursePrices_Filter()
                {
                    Field = field,
                    Criteria = criteria
                }
            };
        }
        #endregion

        #region CourseHeadersWithPrices
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
                            if (criteria.StartsWith("|"))
                            {
                                criteria = criteria.Substring(1);
                            }
                            if (criteria.EndsWith("|"))
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
                Services.LogService.Error("Fejl i sammensætning af kurser og priser.", exception);
                throw exception;
            }

            return list;
        }
        #endregion

        #region CourseHeaderWithPrices
        /// <summary>
        /// Henter kursus med detaljeret info og pris fra navision. Udfra kursus id/nummer.
        /// </summary>
        /// <param name="courseNumber">Kursus id/nummer</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <returns></returns>
        public static CourseHeaderWithPrice GetCourseHeaderWithPrices(string courseNumber, string subscriptionKey, string accountKey = null)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };
            return GetCourseHeaderWithPrices(courseNumber, apiKeys);
        }
        internal static CourseHeaderWithPrice GetCourseHeaderWithPrices(string courseNumber, ApiManagerCredentials credentials)
        {
            try
            {
                var courseHeaderWithPrices = new CourseHeaderWithPrice();
                var courseHeader = CourseService.GetCourseHeader(courseNumber, credentials);
                if (courseHeader != null)
                {
                    courseHeaderWithPrices.Info = courseHeader;
                    courseHeaderWithPrices.Prices = CourseService.GetCoursePricesByType(credentials.SubscriptionKey, credentials.AccountKey, courseHeader.Course_Type_No);
                }

                return courseHeaderWithPrices;
            }
            catch (Exception exception)
            {
                Services.LogService.Error("Fejl i sammensætning af kursus og priser.", exception);
                return null;
            }
        }
        #endregion

        #region CourseEnrollment
        /// <summary>
        /// Opretter alle kursister i listen af CourseEnrollment.
        /// </summary>
        /// <param name="courseEnrollments">Hver tilmelding er 1 CourseEnrollment. Så 7 personer til samme kursus vil være 7 CourseEnrollments.</param>
        /// <param name="subscriptionKey">Opret dig på Api Manageren for at få en key. [Påkrævet]</param>
        /// <param name="accountKey">Regnskabs id'en fra navision. [Optional]</param>
        /// <returns></returns>
        public static bool CreateCourseEnrollment(IEnumerable<CourseEnrollment> courseEnrollments, string subscriptionKey, string accountKey = null)
        {
            var apiKeys = new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey

            };
            return CreateCourseEnrollment(courseEnrollments, apiKeys);
        }
        internal static bool CreateCourseEnrollment(IEnumerable<CourseEnrollment> courseEnrollments, ApiManagerCredentials credentials)
        {
            try
            {
                var navCourseEnrollments = new CourseEnrollments1 { CourseEnrollment = courseEnrollments.ToArray() };
                var courseCreateService = new CourseEnrollments
                {
                    ApiCredentials = credentials
                };
                courseCreateService.CallCourseEnrollments(ref navCourseEnrollments);
                courseCreateService.Dispose();
                return true;
            }
            catch (Exception exception)
            {
                Services.LogService.Error("Fejl i oprettelse af kursusister.", exception);
                throw exception;
                return false;
            }
        }
        #endregion
    }
}