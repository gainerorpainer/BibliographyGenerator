using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographyGenerator
{
    internal abstract class International
    {
        public abstract string LastAccessed { get; }
    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class InternationalAttribute : Attribute
    {
        // This is a positional argument
        public InternationalAttribute(string languageTag)
        {
            LanguageTag = languageTag;
        }

        public string LanguageTag { get; }
    }

    [International("de")]
    internal class International_DE : International
    {
        public override string LastAccessed => "Letzter Zugriff am";
    }

    [International("en")]
    internal class International_EN : International
    {
        public override string LastAccessed => "Accessed";
    }
}
