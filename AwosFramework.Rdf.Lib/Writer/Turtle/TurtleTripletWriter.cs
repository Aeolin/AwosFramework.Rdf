using AwosFramework.Rdf.Lib.Core;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AwosFramework.Rdf.Lib.Writer.Turtle
{
	internal class TurtleTripletWriter : ITripletWriter
	{
		public Stream BaseStream { get; init; }
		public Encoding Encoding { get; init; }
		private readonly TextWriter _writer;
		private readonly ConcurrentDictionary<string, IRI> _prefixMap = new ConcurrentDictionary<string, IRI>();
		private readonly List<IRI> _iris = new List<IRI>();
		private IRI _base;

		private readonly ObjectPool<TurtleObjectListWriter> _listWriterPool = ObjectPool.Create<TurtleObjectListWriter>();
		private readonly ObjectPool<TurtleSubjectContext> _subjectWriterPool = ObjectPool.Create<TurtleSubjectContext>();


		public TurtleTripletWriter(Stream baseStream, Encoding encoding)
		{
			BaseStream=baseStream;
			Encoding=encoding;
			_writer = StreamWriter.Synchronized(new StreamWriter(baseStream, encoding));
		}

		public TurtleTripletWriter(TextWriter writer)
		{
			_writer = StreamWriter.Synchronized(writer);
		}

		protected void WriteSynced(string @string = "")
		{
			_writer.Write(@string);
		}

		protected void WriteLineSynced(string @string = "")
		{
			_writer.WriteLine(@string);
		}

		public ITripletWriter DefineBase(IRI iri)
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

				WriteLineSynced($"@base {iri} .");
			}

			return this;
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

		public ISubjectWriter BeginSubject(IRI identifier)
		{
			var instance = _subjectWriterPool.Get();
			instance.Reset(identifier.ToString());
			return instance;
		}

		public ISubjectWriter BeginSubject(IRI identifier, string id)
		{
			var instance = _subjectWriterPool.Get();
			instance.Reset(identifier.Concat(id));
			return instance;
		}

		public ITripletWriter EndSubject(ISubjectWriter subject)
		{
			var content = subject.ToString();
			WriteLineSynced(content+" .\n");
			_subjectWriterPool.Return((TurtleSubjectContext)subject);
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, IRI baseObject, string objectId)
		{
			WriteLineSynced($"{subject} {predicate} {baseObject.Concat(objectId)} .");
			return this;
		}

		public ITripletWriter Write(IRI baseSubject, string subjectId, IRI predicate, IRI @object)
		{
			WriteLineSynced($"{baseSubject.Concat(subjectId)} {predicate} {@object} .");
			return this;
		}

		public ITripletWriter Write(IRI baseSubject, string subjectId, IRI predicate, IRI baseObject, string objectId)
		{
			WriteLineSynced($"{baseSubject.Concat(subjectId)} {predicate} {baseObject.Concat(objectId)} .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, IRI @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, string @object)
		{
			WriteLineSynced($"{subject} {predicate} \"{TurtleUtils.Escape(@object)}\" .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, decimal @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, double @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, bool @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public void Dispose()
		{
			_writer.Flush();
			_writer.Dispose();
		}

		public ITripletWriter Write(IRI subject, IRI predicate, long @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public ITripletWriter Write(IRI subject, IRI predicate, ulong @object)
		{
			WriteLineSynced($"{subject} {predicate} {@object} .");
			return this;
		}

		public IObjectListWriter BeginObjectList(IRI subject, IRI predicate)
		{
			var builder = new StringBuilder();
			builder.Append($"{subject} {predicate} ");
			var writer = _listWriterPool.Get();
			writer.Reset(builder);
			return writer;
		}

		public ITripletWriter EndObjectList(IObjectListWriter writer)
		{
			if (writer is TurtleObjectListWriter turtleListWriter)
			{
				turtleListWriter.Builder.Append(" .");
				WriteLineSynced(turtleListWriter.Builder.ToString());
				_listWriterPool.Return(turtleListWriter);
				return this;
			}
			else
			{
				throw new ArgumentException($"expected writer to be instance of {nameof(TurtleObjectListWriter)}", nameof(writer));
			}
		}

		public ITripletWriter Write(IRI subject, IRI predicate, object @object)
		{
			WriteLineSynced($"{subject} {predicate} {TurtleUtils.ConvertToLiteral(@object)} .");
			return this;
		}

		public ITripletWriter WriteRaw(IRI subject, IRI predicate, string rawValue)
		{
			WriteLineSynced($"{subject} {predicate} {rawValue} .");
			return this;
		}
	}
}
