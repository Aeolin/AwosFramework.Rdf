using AwosFramework.Rdf.Lib.Writer.Turtle;
using AwosFramework.Rdf.Lib.Core;

namespace AwosFramework.Rdf.Lib.Writer
{
	public interface ISubjectContext
	{
		string ToString();
		ISubjectContext WriteLiteral(IRI predicate, bool @bool);
		ISubjectContext WriteLiteral(IRI predicate, decimal number);
		ISubjectContext WriteLiteral(IRI predicate, double number);
		ISubjectContext WriteLiteral(IRI predicate, int number);
		ISubjectContext WriteLiteral(IRI predicate, string literal);
		ISubjectContext WriteLiteral(IRI predicate, IRI baseIri, string id);
		ISubjectContext WriteLiteral(IRI predicate, IEnumerable<IRI> literals);
		ISubjectContext WriteLiteral(IRI predicate, IEnumerable<object> objects);
		ISubjectContext WriteType(IRI type);
	}
}