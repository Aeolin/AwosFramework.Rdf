using AwosFramework.Rdf.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal class TurtleObjectListWriter : IObjectListWriter
	{
		public StringBuilder Builder { get; private set; }
		private bool _firstSeparator;

		public void Reset(StringBuilder builder)
		{
			Builder = builder;
			_firstSeparator = true;
		}

		private void AppendSeparator()
		{
			if (_firstSeparator)
				_firstSeparator = false;
			else
				Builder.Append(", ");
		}

		public IObjectListWriter Write(bool @bool)
		{
			AppendSeparator();
			Builder.Append(@bool);
			return this;
		}

		public IObjectListWriter Write(decimal number)
		{
			AppendSeparator();
			Builder.Append(number);
			return this;
		}

		public IObjectListWriter Write(double number)
		{
			AppendSeparator();
			Builder.Append(number);
			return this;
		}

		public IObjectListWriter Write(ulong number)
		{
			AppendSeparator();
			Builder.Append(number);
			return this;
		}

		public IObjectListWriter Write(long number)
		{
			AppendSeparator();
			Builder.Append(number);
			return this;
		}

		public IObjectListWriter Write(string literal)
		{
			AppendSeparator();
			Builder.Append($"\"{TurtleUtils.Escape(literal)}\"");
			return this;
		}

		public IObjectListWriter Write(IRI baseIri, string id)
		{
			AppendSeparator();
			Builder.Append(baseIri.Concat(id));
			return this;
		}

		public IObjectListWriter Write(IRI iri)
		{
			AppendSeparator();
			Builder.Append(iri.ToString());
			return this;
		}

		public IObjectListWriter Write(object obj)
		{
			AppendSeparator();
			Builder.Append(TurtleUtils.ConvertToLiteral(obj));
			return this;
		}

		public IObjectListWriter WriteRaw(string rawValue)
		{
			AppendSeparator();
			Builder.Append(rawValue);
			return this;
		}

		public IObjectListWriter WriteSchemaType(IRI schema)
		{
			Builder.Append($"^^{schema}");
			return this;
		}

		public IObjectListWriter WriteSchemaType(IRI baseIri, string identifier)
		{
			Builder.Append($"^^{baseIri.Concat(identifier)}");
			return this;
		}
	}
}
