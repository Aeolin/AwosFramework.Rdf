using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.RDF.Lib.Core
{
	public class RelativeIri : Iri
	{
		public bool IsBase => string.IsNullOrEmpty(Prefix);
		public string Prefix { get; init; }
		public static bool operator ==(RelativeIri a, RelativeIri b) => a.Value == b.Value && a.Prefix == b.Prefix;
		public static bool operator !=(RelativeIri a, RelativeIri b) => a.Value != b.Value || a.Prefix != b.Prefix;

		public override string ToString() => $":{Prefix}";

		public RelativeIri(string value, string prefix) : base(value)
		{
			Prefix = prefix;
		}

		public override bool Equals(object obj)
		{
			return obj is RelativeIri iri&&
						 base.Equals(obj)&&
						 Value==iri.Value&&
						 Prefix==iri.Prefix;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), Value, Prefix);
		}
	}
}
