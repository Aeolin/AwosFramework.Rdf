using AwosFramework.Rdf.Lib.Core;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal static class TurtleUtils
	{
		private static readonly FrozenSet<char> ESCAPEABLE_CHARS = new HashSet<char> { '\r', '\n', '\f', '\b', '\"', '\t', '\\' }.ToFrozenSet();


		public static string Escape(string @string)
		{
			var res = "";
			foreach (var c in @string)
				if (ESCAPEABLE_CHARS.Contains(c))
					res += $"\\{c}";
				else
					res += c;

			return res;
		}

		public static string ConvertToLiteral(object obj)
		{
			if (obj is IRI iri)
				return iri.ToString();
			else if (obj is string @string)
				return $"\"{TurtleUtils.Escape(@string)}\"";
			else if (obj is double @double)
				return @double.ToString("0.##E+00");
			else if (obj.GetType().IsPrimitive)
				return obj.ToString();
			else if (obj.GetType().IsEnum || obj.GetType().IsValueType)
				return $"\"{TurtleUtils.Escape(obj.ToString())}\"";
			else
				throw new ArgumentException("only primitives or IRI allowed");
		}
	}
}
