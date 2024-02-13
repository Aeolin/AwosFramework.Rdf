using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Core
{
	public class IRI : IEquatable<IRI>
	{
		public string LiteralValue => ToString();

		public string Value { get; init; }
		public string Prefix { get; private set; }
		public string BasedValue { get; private set; }

		public bool IsPrefixed => Prefix != null;
		public bool IsBased { get; private set; }
		public IRI Base { get; private set; }
		public bool IsImmutable { get; init; } = false;

		internal void DefinePrefix(string prefix)
		{
			this.Prefix = prefix;
		}

		public IRI(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("IRI cannot be null or whitespace.", nameof(value));
			}

			if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
			{
				throw new ArgumentException("Invalid IRI format.", nameof(value));
			}

			Value = value;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as IRI);
		}

		public bool Equals(IRI other)
		{
			return other != (IRI)null && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return IsPrefixed ? Prefix : (IsBased ? BasedValue : $"<{Value}>");
		}

		public string Concat(string id) 
		{
			if (IsImmutable)
				throw new InvalidOperationException($"Can't concatinate immutable IRI's");

			return IsPrefixed ? $"{this.Prefix}:{id}" : (IsBased ? $"{this.BasedValue}{id}" : $"<{this.BasedValue}{id}>");
		}

		internal void Rebase(IRI iri)
		{
			if (iri != (IRI)null && Value.StartsWith(iri.Value))
			{
				Base = iri;
				BasedValue = ":" + Value.Substring(iri.Value.Length);
				IsBased = true;
			}
			else
			{
				Base = null;
				BasedValue = Value;
				IsBased = false;
			}
		}

		public IRI OfPrefixed(string @string)
		{
			if (IsImmutable)
				throw new InvalidOperationException($"Can't concatinate immutable IRI's");

			var res = new IRI(Concat(@string)) { IsImmutable = true };
			if (IsPrefixed)
				res.Prefix = $"{Prefix}:{@string}";

			res.Rebase(Base);
			return res;
		}

		public static IRI operator +(IRI left, string right) => left.OfPrefixed(right);

		public static bool operator ==(IRI left, IRI right)
		{
			return EqualityComparer<IRI>.Default.Equals(left, right);
		}

		public static bool operator !=(IRI left, IRI right)
		{
			return !(left == right);
		}

		public static bool operator ==(IRI left, string right)
		{
			return EqualityComparer<string>.Default.Equals(left.Value, right);
		}

		public static bool operator !=(IRI left, string right)
		{
			return !(left == right);
		}
	}

}
