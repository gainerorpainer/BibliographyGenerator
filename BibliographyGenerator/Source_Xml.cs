using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibliographyGenerator
{
    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class Source_Xml
    {
        public string Tag { get; set; }
        public string SourceType { get; set; }
        public string Guid { get; set; }
        public AuthorContainer Author { get; set; }
        public int RefOrder { get; set; }
        public string DOI { get; set; }
    }


    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class Book_Xml : Source_Xml
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string City { get; set; }
        public string Publisher { get; set; }
    }

    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class ConferenceProceedings_Xml : Source_Xml
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string City { get; set; }
        public string ConferenceName { get; set; }
    }

    /// <summary>
    /// Exactly the same as Book
    /// </summary>
    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class Report_Xml : Book_Xml
    { }


    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class DocumentFromInternetSite_Xml : Source_Xml
    {
        public int LCID { get; set; }
        public string Title { get; set; }

        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }

        public int YearAccessed { get; set; }
        public string MonthAccessed { get; set; }
        public int DayAccessed { get; set; }

        public string URL { get; set; }

        static internal int MonthToNumber(string month, CultureInfo culture)
        {
            var pseudoDate = DateTime.ParseExact(month, "MMMM", culture);
            return pseudoDate.Month;
            //switch (MonthAccessed)
            //{
            //    case "January":
            //        return 1;
            //    case "February":
            //        return 2;
            //    case "March":
            //        return 3;
            //    case "April":
            //        return 4;
            //    case "May":
            //        return 5;
            //    case "June":
            //        return 6;
            //    case "July":
            //        return 7;
            //    case "August":
            //        return 8;
            //    case "September":
            //        return 9;
            //    case "October":
            //        return 10;
            //    case "November":
            //        return 11;
            //    case "December":
            //        return 12;
            //    default:
            //        throw new NotImplementedException();
            //}
        }
    }

    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class InternetSite_Xml : DocumentFromInternetSite_Xml
    { }

    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class ArticleInAPeriodical_Xml : Source_Xml
    {
        public string Title { get; set; }
        public string PeriodicalTitle { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public string Pages { get; set; }
    }


    public class Person
    {
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Author
    {
        public List<Person> NameList { get; set; }
    }

    public class AuthorContainer
    {
        public Author Author { get; set; }
    }
}
