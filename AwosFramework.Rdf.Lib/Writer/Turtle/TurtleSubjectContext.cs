using AwosFramework.Rdf.Lib.Core;
using AwosFramework.Rdf.Lib.Utils;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal class TurtleSubjectContext : ISubjectWriter
	{
		private static ObjectPool<TurtleObjectListWriter> _listWriters = ObjectPool.Create<TurtleObjectListWriter>();
		private const string IDENT = "  ";
		private readonly StringBuilder _builder = new StringBuilder();
		private string _identifier;
		private bool _first = true;

		public override string ToString() => _builder.ToString();


		internal void Reset(string identifier)
		{
			_builder.Clear();
			_builder.AppendLine(identifier.ToString());
			_first = true;
		}

		protected void AppendSeparator()
		{
			if (_first)
			{
				_first = false;
			}
			else
			{
				_builder.AppendLine(" ;");
			}
		}

		public ISubjectWriter Write(IRI type)
		{
			AppendSeparator();
			_builder.Append(IDENT);
			_builder.Append("a ");
			_builder.Append(type.ToString());
			return this;
		}

		public ISubjectWriter WriteSubjectType(IRI type, string identifier)
		{
			AppendSeparator();
			_builder.Append(IDENT);
			_builder.Append("a ");
			_builder.Append(type.Concat(identifier));
			return this;
		}

		protected void WriteLiteralHeader(IRI predicate)
		{
			AppendSeparator();
			_builder.Append(IDENT);
			_builder.Append(predicate.ToString());
			_builder.Append(" ");
		}

		public ISubjectWriter Write(IRI predicate, string literal)
		{
			if (literal ==null)
				return this;

			WriteLiteralHeader(predicate);
			_builder.Append($"\"{TurtleUtils.Escape(literal)}\"");
			return this;
		}

		public ISubjectWriter Write(IRI predicate, long number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, ulong number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, decimal number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, double number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, bool @bool)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(@bool);
			return this;
		}

		public ISubjectWriter WriteLiteral(IRI predicate, IRI iri)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(iri);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, IRI baseIri, string id)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(baseIri.Concat(id));
			return this;
		}

		public ISubjectWriter WriteRaw(IRI predicate, string content)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(content);
			return this;
		}

		public ISubjectWriter Write(IRI predicate, object obj)
		{
			if (obj == null)
				return this;

			if (obj is not string && obj is IEnumerable enumerable)
			{
				var items = enumerable.Cast<object>();
				var list = BeginObjectList(predicate);
				foreach (var item in items)
					list.Write(item);

				EndObjectList(list);
			}
			else
			{
				WriteLiteralHeader(predicate);
				_builder.Append(TurtleUtils.ConvertToLiteral(obj));
			}

			return this;
		}

		public IObjectListWriter BeginObjectList(IRI predicate)
		{
			WriteLiteralHeader(predicate);
			var writer = _listWriters.Get();
			writer.Reset(_builder);
			return writer;
		}

		public ISubjectWriter EndObjectList(IObjectListWriter writer)
		{
			if (writer is TurtleObjectListWriter turtleListWriter)
			{
				_listWriters.Return(turtleListWriter);
				return this;
			}
			else
			{
				throw new ArgumentException($"expected writer to be instance of {nameof(TurtleObjectListWriter)}", nameof(writer));
			}
		}

		public ISubjectWriter WriteSchemaType(IRI type)
		{

			_builder.Append($"^^{type}");
			return this;
		}

		public ISubjectWriter WriteSchemaType(IRI baseIri, string identifier)
		{
			ArgumentNullException.ThrowIfNull(identifier);
			_builder.Append($"^^{baseIri.Concat(identifier)}");
			return this;
		}

	}
}
