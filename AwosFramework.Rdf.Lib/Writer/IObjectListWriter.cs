using AwosFramework.Rdf.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer
{
	public interface IObjectListWriter
	{
		IObjectListWriter Write(bool @bool);
		IObjectListWriter Write(decimal number);
		IObjectListWriter Write(double number);
		IObjectListWriter Write(ulong number);
		IObjectListWriter Write(long number);
		IObjectListWriter Write(string literal);
		IObjectListWriter Write(IRI baseIri, string id);
		IObjectListWriter Write(IRI iri);
		IObjectListWriter Write(object obj);
		IObjectListWriter WriteRaw(string rawValue);
		IObjectListWriter WriteSchemaType(IRI schema);
		IObjectListWriter WriteSchemaType(IRI baseIri, string identifier);
	}
}
