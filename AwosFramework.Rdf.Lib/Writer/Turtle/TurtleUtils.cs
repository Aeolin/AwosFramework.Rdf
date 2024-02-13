using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal static class TurtleUtils
	{
		private static readonly char[] ESCAPEABLE_CHARS = new[] { '\r', '\n', '\f', '\b', '\"', '\t', '\\', '~', '.', ',', '$', '&', '\'', '+', '(', ')', '#', '@', '%', '_', '=', '?', ';', '/' };
		private static Regex MATCHER = new Regex($"[{string.Join("", ESCAPEABLE_CHARS)}]", RegexOptions.Compiled);


		public static string Escape(string @string)
		{
			return MATCHER.Replace(@string, x => $"\\{x.Value}");
		}
	}
}
