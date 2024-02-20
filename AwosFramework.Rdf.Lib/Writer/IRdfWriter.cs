using AwosFramework.Rdf.Lib.Writer.Turtle;
using AwosFramework.Rdf.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer
{
    public interface IRdfWriter : IDisposable
	{
		public IRI DefineIri(string iri);
		public IRI DefineIri(string iri, string prefix);
		public ISubjectContext BeginSubject(IRI identifier);
		public ISubjectContext BeginSubject(IRI baseIdentifier, string id);
		public void EndSubject(ISubjectContext subject);
		public void DefineBase(IRI @base);
		public void WriteTriplet(IRI subject, IRI predicate, IRI @object);
		public void WriteTriplet(IRI subject, IRI predicate, string @object);
		public void WriteTriplet(IRI subject, IRI predicate, long @object);
		public void WriteTriplet(IRI subject, IRI predicate, ulong @object);
		public void WriteTriplet(IRI subject, IRI predicate, decimal @object);
		public void WriteTriplet(IRI subject, IRI predicate, double @object);
		public void WriteTriplet(IRI subject, IRI predicate, bool @object);
	}
}
