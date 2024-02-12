using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.RDF.Lib
{
	public class RdfWriter
	{
		public Stream BaseStream { get; init; }
		public Encoding Encoding { get; init; }
		private readonly StreamWriter _writer;

		public RdfWriter(Stream baseStream, Encoding encoding)
		{
			BaseStream=baseStream;
			Encoding=encoding;
		}

	}
}
