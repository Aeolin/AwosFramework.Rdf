using AwosFramework.Rdf.Lib.Writer;
using AwosFramework.Rdf.Lib.Writer.Turtle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwosFramework.Rdf.Lib
{
	public static class WriterFactory
	{
		public static ITripletWriter TurtleWriter(string filePath, Encoding? enc = null)
		{
			var dir = Directory.GetParent(filePath).FullName;
			Directory.CreateDirectory(dir);
			var stream = File.Create(filePath);
			return TurtleWriter(stream, enc);
		}

		public static ITripletWriter TurtleWriter(Stream stream, Encoding? enc = null)
		{
			return new TurtleTripletWriter(stream, enc ?? Encoding.UTF8);
		}

		public static ITripletWriter TurtleWriter(TextWriter writer)
		{
			return new TurtleTripletWriter(writer);
		}
	}
}
