using AwosFramework.Rdf.Lib.Writer.Turtle;
using AwosFramework.Rdf.Lib.Core;

namespace AwosFramework.Rdf.Lib.Writer
{
	public interface ISubjectWriter
	{
		ISubjectWriter Write(IRI predicate, bool @bool);
		ISubjectWriter Write(IRI predicate, decimal number);
		ISubjectWriter Write(IRI predicate, double number);
		ISubjectWriter Write(IRI predicate, ulong number);
		ISubjectWriter Write(IRI predicate, long number);
		ISubjectWriter Write(IRI predicate, string literal);
		ISubjectWriter Write(IRI predicate, IRI baseIri, string id);
		ISubjectWriter Write(IRI predicate, object obj);
		ISubjectWriter WriteRaw(IRI predicate, string content);
		ISubjectWriter Write(IRI type);
		ISubjectWriter WriteSubjectType(IRI baseIri, string id);
		ISubjectWriter WriteSchemaType(IRI type);
		ISubjectWriter WriteSchemaType(IRI baseIri, string identifier);
		IObjectListWriter BeginObjectList(IRI predicate);
		ISubjectWriter EndObjectList(IObjectListWriter writer);
	}
}