using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.RDF.Lib.Core
{
	public class IriContext
	{
		public RelativeIri BaseIri { get; private set; }
		private readonly HashSet<Iri> _iris = new HashSet<Iri>();
		public IEnumerable<Iri> Iris => _iris;

		public void PushBaseFragment(string fragment)
		{
		}
		
	}
}
