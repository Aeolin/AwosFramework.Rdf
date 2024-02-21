using AwosFramework.Rdf.Lib.Core;
using AwosFramework.Rdf.Lib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal class TurtleSubjectContext : ISubjectContext
	{
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

		public ISubjectContext WriteType(IRI type)
		{
			AppendSeparator();
			_builder.Append(IDENT);
			_builder.Append("a ");
			_builder.Append(type.ToString());
			return this;
		}

		public ISubjectContext WriteType(IRI type, string identifier)
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

		public ISubjectContext WriteLiteral(IRI predicate, string literal)
		{
			WriteLiteralHeader(predicate);
			_builder.Append($"\"{literal}\"");
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, long number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, ulong number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, decimal number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, double number)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(number.ToString("0.##E+00"));
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, bool @bool)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(@bool);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, IEnumerable<object> objects)
		{
			WriteLiteralHeader(predicate);
			bool first = true;
			foreach (var obj in objects)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					_builder.Append(", ");
				}

				AppenObjectValue(obj);
			}

			return this;
		}

		private void AppenObjectValue(object obj)
		{
			if (obj is IRI iri)
				_builder.Append(iri.ToString());
			else if (obj is string @string)
				_builder.Append($"\"{TurtleUtils.Escape(@string)}\"");
			else if (obj is double @double)
				_builder.Append(@double.ToString("0.##E+00"));
			else if (obj.GetType().IsPrimitive)
				_builder.Append(obj.ToString());
			else if (obj.GetType().IsEnum || obj.GetType().IsValueType)
				_builder.Append($"\"{TurtleUtils.Escape(obj.ToString())}\"");
			else
				throw new ArgumentException("only primitives or IRI allowed");
		}

		public ISubjectContext WriteLiteral(IRI predicate, IEnumerable<IRI> literals)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(string.Join(", ", literals.Select(x => x.LiteralValue)));
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, IRI iri)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(iri);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, IRI baseIri, string id)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(baseIri.Concat(id));
			return this;
		}

		public ISubjectContext WriteLiteralUnchecked(IRI predicate, string content)
		{
			WriteLiteralHeader(predicate);
			_builder.Append(content);
			return this;
		}

		public ISubjectContext WriteLiteral(IRI predicate, object obj)
		{
			if (obj == null)
				return this;

			if (obj is not string && obj is IEnumerable enumerable)
			{
				var objs = enumerable.Cast<object>();
				WriteLiteral(predicate, objs);
			}
			else
			{
				WriteLiteralHeader(predicate);
				AppenObjectValue(obj);
			}

			return this;
		}
	}
}
