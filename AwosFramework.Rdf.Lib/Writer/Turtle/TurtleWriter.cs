using AwosFramework.Rdf.Lib.Core;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal class TurtleWriter : IRdfWriter
	{
		public Stream BaseStream { get; init; }
		public Encoding Encoding { get; init; }
		private readonly TextWriter _writer;
		private readonly Dictionary<string, IRI> _prefixMap = new Dictionary<string, IRI>();
		private readonly List<IRI> _iris = new List<IRI>();
		private IRI _base;

		private ObjectPool<TurtleSubjectContext> _contextPool = ObjectPool.Create<TurtleSubjectContext>();


		public TurtleWriter(Stream baseStream, Encoding encoding)
		{
			BaseStream=baseStream;
			Encoding=encoding;
			_writer = StreamWriter.Synchronized(new StreamWriter(baseStream, encoding));
		}

		protected void WriteSynced(string @string)
		{
			_writer.Write(@string);
		}

		protected void WriteLineSynced(string @string)
		{
			_writer.WriteLine(@string);
		}

		public void DefineBase(IRI iri)
		{
			if (iri != _base)
			{
				_base = iri;
				_iris.ForEach(x =>
				{
					if (x != iri)
					{
						x.Rebase(iri);
					}
				});

				WriteLineSynced($"@base {iri.ToString()} .");
			}
		}

		public IRI DefineIri(string iri)
		{
			var res = new IRI(iri);
			res.Rebase(_base);
			_iris.Add(res);
			return res;
		}

		public IRI DefineIri(string iri, string prefix)
		{
			var res = DefineIri(iri);
			if (_prefixMap.TryGetValue(prefix, out var existing) && existing != iri)
				throw new ArgumentException($"Prefix already defined for other iri {existing.Value}");

			res.DefinePrefix(prefix);
			res.Rebase(_base);
			_prefixMap[prefix] = res;
			_iris.Add(res);
			WriteLineSynced($"@prefix {prefix}: <{iri}> .");
			return res;
		}

		public ISubjectContext BeginSubject(IRI identifier)
		{
			var instance = _contextPool.Get();
			instance.Reset(identifier.ToString());
			return instance;
		}

		public ISubjectContext BeginSubject(IRI identifier, string id)
		{
			var instance = _contextPool.Get();
			instance.Reset(identifier.Concat(id));
			return instance;
		}

		public void EndSubject(ISubjectContext subject)
		{
			var content = subject.ToString();
			WriteLineSynced(content+" .\n");
			_contextPool.Return((TurtleSubjectContext)subject);
		}

		public void WriteTriplet(IRI subject, IRI predicate, IRI baseObject, string objectId)
		{
			WriteLineSynced($"{subject} {predicate} {baseObject.Concat(objectId)} .");
		}

		public void WriteTriplet(IRI baseSubject, string subjectId, IRI predicate, IRI @object)
		{
			WriteLineSynced($"{baseSubject.Concat(subjectId)} {predicate} {@object} .");
		}

		public void WriteTriplet(IRI baseSubject, string subjectId, IRI predicate, IRI baseObject, string objectId)
		{
			WriteLineSynced($"{baseSubject.Concat(subjectId)} {predicate} {baseObject.Concat(objectId)} .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, IRI @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, string @object)
		{
			WriteLineSynced($"{subject} {predicate} \"{TurtleUtils.Escape(@object)}\" .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, decimal @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, double @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, bool @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object:0.##E+00} .");
		}

		public void Dispose()
		{
			_writer.Flush();
			_writer.Dispose();
		}

		public void WriteTriplet(IRI subject, IRI predicate, long @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
		}

		public void WriteTriplet(IRI subject, IRI predicate, ulong @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
		}


	}
}
