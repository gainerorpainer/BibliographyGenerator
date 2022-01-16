using System;
using System.Collections.Generic;
using System.Globalization;
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
        public CultureInfo Culture { get; }

        public SpringerBibliography(International international, string internationalTag) : base(international)
        {
            InternationalTag = internationalTag;
            Culture = CultureInfo.GetCultureInfo(internationalTag);
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
                    sb.Append($"{report.Title}. {report.Publisher}, {report.City}, ({report.Year})");
                    break;

                case Book_Xml book:
                    sb.Append($"{authors} ({book.Year}) {book.Title}. {book.Publisher}, {book.City}");
                    break;

                case DocumentFromInternetSite_Xml webDoc:
                    string datestr = InternationalTag == "de" ?
                        $"{webDoc.DayAccessed:00}.{DocumentFromInternetSite_Xml.MonthToNumber(webDoc.MonthAccessed, Culture):00}.{webDoc.YearAccessed}" :
                        $"{webDoc.DayAccessed} {webDoc.MonthAccessed} {webDoc.YearAccessed}";
                    sb.Append($"{webDoc.Title}. {webDoc.URL}. {International.LastAccessed} {datestr}");
                    break;

                case ArticleInAPeriodical_Xml article:
                    sb.Append($"{authors} ({article.Year}) {article.Title}. {article.PeriodicalTitle}:{article.Pages}");
                    break;

                case ConferenceProceedings_Xml conferenceProceedings:
                    sb.Append($"{authors} ({conferenceProceedings.Year}) {conferenceProceedings.Title}. {conferenceProceedings.ConferenceName}");
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!string.IsNullOrEmpty(entry.DOI))
                sb.Append(". https://doi.org/" + entry.DOI);

            sb.AppendLine();
        }
    }
}
