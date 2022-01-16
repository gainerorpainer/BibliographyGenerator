using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibliographyGenerator
{
    internal abstract class BibliographyBase
    {
        public International International { get; private set; }

        protected BibliographyBase(International international)
        {
            International = international;
        }

        public abstract string Serialize(IEnumerable<Source_Xml> sources);
    }
}