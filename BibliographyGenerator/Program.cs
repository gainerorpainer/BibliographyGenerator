using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace BibliographyGenerator
{
    class Program
    {
        static readonly Dictionary<string, Type> _XmlTypeMap;

        static Program()
        {
            // Setup a map that can determine the destination type from a string identifier
            _XmlTypeMap =
                // From all assemblies, from all types in those assemblies, find types that can be downcasted (inherit from) Source_Xml
                FindDerrivedTypes<Source_Xml>()
                // key = name until first '_', value = type itself
                .ToDictionary(x => x.Name.Split('_').First(), x => x);
        }

        private static IEnumerable<Type> FindDerrivedTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => typeof(T).IsAssignableFrom(x));
        }

        [STAThread]
        static void Main(string[] args)
        {
            International _International;
            string internationalArg = args.FirstOrDefault(x => x.StartsWith("/international:"));
            string internationalTag = internationalArg is null ? "en" : internationalArg.Split(':').Skip(1).First();

            // load international?
            var internationalType = FindDerrivedTypes<International>().Single(
                x => ((InternationalAttribute)x.GetCustomAttributes(typeof(InternationalAttribute), false).FirstOrDefault())?.LanguageTag == internationalTag);

            _International = (International)Activator.CreateInstance(internationalType);


            // parse input file
            var sources = new List<Source_Xml>();
            ParseSources(args.First(), sources);

            // copy to clipboard?
            string serialize = new SpringerBibliography(_International, internationalTag).Serialize(sources);

            // write
            Console.Write(serialize);

            if (args.Contains("/stay"))
            {
                Console.WriteLine("Enter to exit...");
                Console.ReadLine();
            }
        }

        private static void ParseSources(string filePath, List<Source_Xml> sources)
        {
            foreach (var xml in File.ReadAllLines(filePath, Encoding.UTF8))
            {
                // trim and substitute
                var trimmedxml = xml.Trim('"').Replace("\"\"", "\"");

                // parse base
                Source_Xml model = ParseXmlString<Source_Xml>(trimmedxml);

                // parse derrived
                Type destinationType = _XmlTypeMap[model.SourceType];

                // Now thats a funny way how to call a generic methdod
                var methodInfo = typeof(Program).GetMethod(nameof(ParseXmlString));
                var methodInvoker = methodInfo.MakeGenericMethod(destinationType);
                Source_Xml derrivedType = (Source_Xml)methodInvoker.Invoke(null, new object[] { trimmedxml });

                sources.Add(derrivedType);
            }

        }

        /// <summary>
        /// Absolutely not high performance function, but supports Unicode :)
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="xmlStr">xml string</param>
        /// <returns></returns>
        public static T ParseXmlString<T>(string xmlStr)
        {
            using (var s = new MemoryStream(Encoding.Unicode.GetBytes(xmlStr)))
            {
                using (var sr = new StreamReader(s, Encoding.Unicode))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
                }
            }
        }
    }
}

