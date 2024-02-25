using AwosFramework.Rdf.Lib.Writer.Turtle;
using AwosFramework.Rdf.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer
{
    public interface ITripletWriter : IDisposable
	{
		public IRI DefineIri(string iri);
		public IRI DefineIri(string iri, string prefix);
		public ISubjectWriter BeginSubject(IRI identifier);
		public ISubjectWriter BeginSubject(IRI baseIdentifier, string id);
		public ITripletWriter EndSubject(ISubjectWriter subject);
		public IObjectListWriter BeginObjectList(IRI subject, IRI predicate);
		public ITripletWriter EndObjectList(IObjectListWriter writer);
		public ITripletWriter DefineBase(IRI @base);
		public ITripletWriter Write(IRI subject, IRI predicate, IRI baseObject, string objectId);
		public ITripletWriter Write(IRI subject, IRI predicate, IRI @object);
		public ITripletWriter Write(IRI baseSubject, string subjectId, IRI predicate, IRI @object);
		public ITripletWriter Write(IRI baseSubject, string subjectId, IRI predicate, IRI baseObject, string objectId);
		public ITripletWriter Write(IRI subject, IRI predicate, string @object);
		public ITripletWriter Write(IRI subject, IRI predicate, long @object);
		public ITripletWriter Write(IRI subject, IRI predicate, ulong @object);
		public ITripletWriter Write(IRI subject, IRI predicate, decimal @object);
		public ITripletWriter Write(IRI subject, IRI predicate, double @object);
		public ITripletWriter Write(IRI subject, IRI predicate, bool @object);
		public ITripletWriter Write(IRI subject, IRI predicate, object @object);
		public ITripletWriter WriteRaw(IRI subject, IRI predicate, string rawValue);
	}
}
