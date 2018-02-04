using ASA.Apim.Library.CourseHeader_Service;
using ASA.Apim.Library.CoursePrices_Service;
using System.Collections.Generic;

namespace ASA.Apim.Library.Entity
{
    public class CourseHeaderWithPrice
    {
        public CourseHeader Info { get; set; }
        public IEnumerable<CoursePrices> Prices { get; set; }

        public CourseHeaderWithPrice() { }

        public CourseHeaderWithPrice(CourseHeader course, IEnumerable<CoursePrices> prices)
        {
            Info = course;
            Prices = prices;
        }
    }
}