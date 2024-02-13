// See https://aka.ms/new-console-template for more information
using AwosFramework.Rdf.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using var writer = WriterFactory.TurtleWriter("test.ttl");
var baseIri = writer.DefineIri("https://example.com/");
writer.DefineBase(baseIri);
var iri = writer.DefineIri("https://example.com/table#", "table");
var typeIri = writer.DefineIri("https://example.com/types#", "type");
var tableType = typeIri.OfPrefixed("table");
var predicates = writer.DefineIri("https://example.com/predicates#", "predicates");
var hasName = predicates.OfPrefixed("hasName");
var hasAge = predicates.OfPrefixed("hasAge");
var hasFriend = predicates.OfPrefixed("hasFriend");
var hasId = predicates.OfPrefixed("hasId");


var subject = writer.BeginSubject(iri, "1");
subject.WriteType(tableType);
subject.WriteLiteral(hasId, 1);
subject.WriteLiteral(hasName, "Max Mustermann");
subject.WriteLiteral(hasAge, 37);
var friends = Enumerable.Range(2, 2).Select(x => iri+x.ToString());
subject.WriteLiteral(hasFriend, friends);
writer.EndSubject(subject);

subject = writer.BeginSubject(iri, "2");
subject.WriteType(tableType);
subject.WriteLiteral(hasId, 2);
subject.WriteLiteral(hasName, "Agatha Martinson");
subject.WriteLiteral(hasAge, 19);
subject.WriteLiteral(hasFriend, iri, "1");
writer.EndSubject(subject);

subject = writer.BeginSubject(iri, "3");
subject.WriteType(tableType);
subject.WriteLiteral(hasId, 3);
subject.WriteLiteral(hasName, "Günther Boomer");
subject.WriteLiteral(hasAge, 76);
subject.WriteLiteral(hasFriend, iri, "2");
writer.EndSubject(subject);

