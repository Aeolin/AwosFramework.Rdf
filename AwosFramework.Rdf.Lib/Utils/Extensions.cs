using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib.Utils
{
	public static class Extensions
	{
		public static bool IsDecimalLike(this Type type) => type == typeof(decimal) || type == typeof(float) || type == typeof(double);
		public static bool IsIntegerLike(this Type type) =>
			type == typeof(byte) || type == typeof(sbyte) ||
			type == typeof(short) || type == typeof(ushort) ||
			type == typeof(int) || type == typeof(uint) ||
			type == typeof(long) || type == typeof(long);

		public static bool IsUIntLike(this Type type) => type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong);
		public static bool IsSIntLike(this Type type) => type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long);
	}
}
