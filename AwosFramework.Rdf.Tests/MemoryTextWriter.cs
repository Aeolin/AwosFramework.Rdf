using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Tests
{
	internal class MemoryTextWriter : TextWriter
	{
		public override Encoding Encoding => Encoding.UTF8;
		private readonly StringBuilder _builder;

		public override string ToString()
		{
			return _builder.ToString();
		}

		public MemoryTextWriter()
		{
			_builder = new StringBuilder();
		}

		public override void Write(char value)
		{
			_builder.Append(value);
		}
	}
}
