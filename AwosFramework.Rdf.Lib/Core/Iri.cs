using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.RDF.Lib.Core
{
	public class Iri
	{
		public Uri Value { get; private set; }

		public Iri(string value)
		{
			Value = new Uri(value);
		}

		public override string ToString()
		{
			return $"<{Value}>";
		}

		public static implicit operator Iri(string value)
		{
			return new Iri(value);
		}
		
		public static implicit operator Uri(Iri iri) => iri.Value;
		public static implicit operator string(Iri iri) => iri.Value.ToString();
		public static bool operator ==(Iri a, Iri b) => a.Value == b.Value;
		public static bool operator !=(Iri a, Iri b) => a.Value != b.Value;


		public override bool Equals(object obj)
		{
			if (obj is Iri)
			{
				return Value == ((Iri)obj).Value;
			}
			return false;
		}
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
