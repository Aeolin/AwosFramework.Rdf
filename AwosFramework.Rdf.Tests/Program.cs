// See https://aka.ms/new-console-template for more information
using AwosFramework.Rdf.Lib;
using AwosFramework.Rdf.Tests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using var memory = new MemoryTextWriter();
using var writer = WriterFactory.TurtleWriter(memory);

var baseIri = writer.DefineIri("https://example.com/");
writer.DefineBase(baseIri);
var iri = writer.DefineIri("https://example.com/table#", "table");
var typeIri = writer.DefineIri("https://example.com/types#", "type");
var tableType = typeIri.Extend("table");
var predicates = writer.DefineIri("https://example.com/predicates#", "predicates");
var hasName = predicates.Extend("hasName");
var hasAge = predicates.Extend("hasAge");
var hasFriend = predicates.Extend("hasFriend");
var hasId = predicates.Extend("hasId");

var subject = writer.BeginSubject(iri, "1");
subject.Write(tableType);
subject.Write(hasId, (object)((short)1));
subject.Write(hasName, "Max Mustermann");
subject.Write(hasAge, 37);
var list = subject.BeginObjectList(hasFriend);
list.Write(iri, "2").Write(iri, "3");
subject.EndObjectList(list);
writer.EndSubject(subject);

subject = writer.BeginSubject(iri, "2");
subject.Write(tableType);
subject.Write(hasId, 2);
subject.Write(hasName, "Agatha Martinson");
subject.Write(hasAge, 19);
subject.Write(hasFriend, iri, "1");
writer.EndSubject(subject);

subject = writer.BeginSubject(iri, "3");
subject.Write(tableType);
subject.Write(hasId, 3);
subject.Write(hasName, "Günther Boomer");
subject.Write(hasAge, 76);
subject.Write(hasFriend, iri, "2");
writer.EndSubject(subject);

Console.WriteLine(memory.ToString());