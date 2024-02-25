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

		/// <summary>
		/// Extends the current IRI by the given string
		/// Also Consider that Extending a Prefixed IRI will make it Immutable
		/// Extending a Prefixed IRI will result in an Complete IRI which string value is IRI:value
		/// Extending a normal IRI will result in a new IRI of the format IRI/value the slash is only added if the IRI doesnt end in one
		/// </summary>
		/// <param name="string">The value to append to the end of the IRI</param>
		/// <returns>Extended IRI</returns>
		/// <exception cref="InvalidOperationException">Throws when trying to extend an immutable iri</exception>
		public IRI Extend(string @string)
		{
			if (IsImmutable)
				throw new InvalidOperationException($"Can't extend immutable IRI's");

			if (IsPrefixed)
			{
				var res = new IRI(Concat(@string)) { IsImmutable = true };
				res.Prefix = $"{Prefix}:{@string}";
				return res;
			}
			else
			{
				var value = Value;
				if (value.EndsWith("/") == false)
					value += "/";

				var res = new IRI($"{Value}{@string}");
				if (IsBased)
					res.Rebase(this.Base);

				return res;
			}
		}

		[Obsolete($"Use {nameof(Extend)} instead")]
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
