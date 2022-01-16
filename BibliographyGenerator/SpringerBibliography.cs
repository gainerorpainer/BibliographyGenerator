using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographyGenerator
{
    class SpringerBibliography : BibliographyBase
    {
        public enum DateFormat
        {

        }

        public string InternationalTag { get; }

        public SpringerBibliography(International international, string internationalTag) : base(international)
        {
            InternationalTag = internationalTag;
        }

        public override string Serialize(IEnumerable<Source_Xml> sources)
        {
            var sb = new StringBuilder();
            foreach (var entry in sources.OrderBy(x => x.RefOrder))
            {
                string authors = string.Join(", ", entry.Author.Author.NameList
                    .Select(SerializePersonToName));

                SbAppendEntry(sb, entry, authors);
            }

            return sb.ToString();
        }

        private static string SerializePersonToName(Person x)
        {
            if (x.First is null)
                return x.Last;
            return $"{x.Last}, {x.First[0]}.";
        }

        private void SbAppendEntry(StringBuilder sb, Source_Xml entry, string authors)
        {
            switch (entry)
            {
                case Report_Xml report:
                    sb.AppendLine($"{report.Title}. {report.Publisher}, {report.City}, ({report.Year})");
                    break;

                case Book_Xml book:
                    sb.AppendLine($"{authors}: {book.Title}. {book.Publisher}, {book.City} ({book.Year}).");
                    break;

                case DocumentFromInternetSite_Xml webDoc:
                    string datestr = InternationalTag == "de" ?
                        $"{webDoc.DayAccessed:00}.{DocumentFromInternetSite_Xml.MonthToNumber(webDoc.MonthAccessed):00}.{webDoc.YearAccessed}" :
                        $"{webDoc.YearAccessed}/{DocumentFromInternetSite_Xml.MonthToNumber(webDoc.MonthAccessed)}/{webDoc.DayAccessed}";
                    sb.AppendLine($"{webDoc.Title}, {webDoc.URL}, {International.LastAccessed} {datestr}.");
                    break;

                case ArticleInAPeriodical_Xml article:
                    sb.AppendLine($"{authors}: {article.Title}. {article.PeriodicalTitle}, {article.Pages} ({article.Year})");
                    break;

                case ConferenceProceedings_Xml conferenceProceedings:
                    sb.AppendLine($"{conferenceProceedings.Title}. {conferenceProceedings.ConferenceName}, {conferenceProceedings.City}, ({conferenceProceedings.Year})");
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
