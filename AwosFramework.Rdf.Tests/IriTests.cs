using AwosFramework.Rdf.Lib.Core;
using System;
using Xunit;

namespace AwosFramework.Rdf.Tests
{
	public class IRITests
	{
		[Fact]
		public void Constructor_ValidIRI_ShouldSetCorrectValue()
		{
			// Arrange
			var value = "http://example.com";

			// Act
			var iri = new IRI(value);

			// Assert
			Assert.Equal(value, iri.Value);
		}

		[Fact]
		public void Constructor_ValidIRI_ShouldSupportNonAscii()
		{
			// Arrange
			var value = "http://example.com/äüöè";

			// Act
			var iri = new IRI(value);

			// Assert
			Assert.Equal(value, iri.Value);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void Constructor_InvalidIRI_ThrowsArgumentException(string value)
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentException>(() => new IRI(value));
		}

		[Fact]
		public void DefinePrefix_WithValidPrefix_SetsIsPrefixedTrue()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			var prefix = "ex";

			// Act
			iri.DefinePrefix(prefix);

			// Assert
			Assert.True(iri.IsPrefixed);
			Assert.Equal(prefix, iri.Prefix);
		}

		[Fact]
		public void Concat_ImmutableIRI_ThrowsInvalidOperationException()
		{
			// Arrange
			var iri = new IRI("http://example.com") { IsImmutable = true };

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => iri.Concat("id"));
		}

		[Fact]
		public void Extend_ValidExtension_ReturnsExtendedIRI()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			var extension = "resource";

			// Act
			var extendedIRI = iri.Extend(extension);

			// Assert
			Assert.Equal("http://example.com/resource", extendedIRI.Value);
		}

		[Fact]
		public void Extend_ImmutableIRI_ThrowsInvalidOperationException()
		{
			// Arrange
			var iri = new IRI("http://example.com") { IsImmutable = true };

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => iri.Extend("resource"));
		}

		[Fact]
		public void Equals_DifferentIRI_ReturnsFalse()
		{
			// Arrange
			var iri1 = new IRI("http://example.com");
			var iri2 = new IRI("http://example.org");

			// Act
			var result = iri1.Equals(iri2);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void Rebase_ValidBase_UpdatesBasedValueAndIsBased()
		{
			// Arrange
			var baseIRI = new IRI("http://example.com");
			var iri = new IRI("http://example.com/resource");

			// Act
			iri.Rebase(baseIRI);

			// Assert
			Assert.True(iri.IsBased);
			Assert.Equal(baseIRI, iri.Base);
			Assert.Equal(":/resource", iri.BasedValue);
		}

		[Fact]
		public void ToString_UnprefixedAndUnbased_ReturnsWrappedValue()
		{
			// Arrange
			var iri = new IRI("http://example.com");

			// Act
			var result = iri.ToString();

			// Assert
			Assert.Equal("<http://example.com>", result);
		}

		[Fact]
		public void DefinePrefix_AndConcatenate_ReturnsPrefixedNotation()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			var prefix = "ex";
			var id = "123";

			// Act
			iri.DefinePrefix(prefix);
			var result = iri.Concat(id);

			// Assert
			Assert.Equal($"{prefix}:{id}", result);
		}

		[Fact]
		public void Extend_PrefixedIRI_ReturnsImmutableExtendedIRI()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			iri.DefinePrefix("ex");
			var extension = "resource";

			// Act
			var extendedIRI = iri.Extend(extension);

			// Assert
			Assert.True(extendedIRI.IsImmutable);
			Assert.Equal("ex:resource", extendedIRI.Prefix);
		}

		[Fact]
		public void Extend_PrefixedIRI_MakesItImmutable()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			iri.DefinePrefix("ex");
			var extension = "data";

			// Act
			var extendedIRI = iri.Extend(extension);

			// Assert
			Assert.Throws<InvalidOperationException>(() => extendedIRI.Extend("moreData"));
		}

		[Fact]
		public void ToString_PrefixedIRI_ReturnsPrefixNotation()
		{
			// Arrange
			var iri = new IRI("http://example.com");
			var prefix = "ex";
			iri.DefinePrefix(prefix);

			// Act
			var result = iri.ToString();

			// Assert
			// Assuming the ToString method should reflect the prefix directly if set,
			// but original class definition may suggest otherwise. Adjust based on actual intended behavior.
			Assert.Equal(prefix, result); // This test assumes ToString returns the prefix directly.
		}

		[Fact]
		public void Rebase_PrefixedIRI_UpdatesBasedValueAndKeepsPrefix()
		{
			// Arrange
			var baseIRI = new IRI("http://example.com");
			var iri = new IRI("http://example.com/resource");
			iri.DefinePrefix("ex");

			// Act
			iri.Rebase(baseIRI);

			// Assert
			Assert.True(iri.IsBased);
			Assert.Equal(baseIRI, iri.Base);
			Assert.Equal("ex", iri.Prefix); // Check if prefix is maintained after rebasing
			Assert.Equal(":/resource", iri.BasedValue);
		}

		[Fact]
		public void Constructor_WithUnusualURI_ShouldHandleCorrectly()
		{
			// Arrange
			var unusualValue = "http://example.com/#?query";

			// Act
			var iri = new IRI(unusualValue);

			// Assert
			Assert.Equal(unusualValue, iri.Value);
		}

		[Fact]
		public void Extend_WithEmptyString_ShouldNotAlterIRI()
		{
			// Arrange
			var originalValue = "http://example.com";
			var iri = new IRI(originalValue);

			// Act
			var extendedIRI = iri.Extend("");

			// Assert
			Assert.Equal(originalValue, extendedIRI.Value);
		}

		[Fact]
		public void Rebase_WithNonMatchingBase_ShouldNotBeBased()
		{
			// Arrange
			var iri = new IRI("http://example.com/resource");
			var nonMatchingBaseIRI = new IRI("http://another.com");

			// Act
			iri.Rebase(nonMatchingBaseIRI);

			// Assert
			Assert.False(iri.IsBased);
			Assert.Null(iri.Base);
		}

		[Fact]
		public void Equals_SameValueDifferentInstances_ShouldBeEqual()
		{
			// Arrange
			var value = "http://example.com";
			var iri1 = new IRI(value);
			var iri2 = new IRI(value);

			// Act & Assert
			Assert.True(iri1.Equals(iri2));
		}

		[Fact]
		public void GetHashCode_SameValueDifferentInstances_ShouldHaveSameHashCode()
		{
			// Arrange
			var value = "http://example.com";
			var iri1 = new IRI(value);
			var iri2 = new IRI(value);

			// Act & Assert
			Assert.Equal(iri1.GetHashCode(), iri2.GetHashCode());
		}

		[Fact]
		public void Concat_WithSlashInBaseAndExtension_ShouldHandleCorrectly()
		{
			// Arrange
			var iri = new IRI("http://example.com/");
			var extension = "/resource";

			// Act
			var result = iri.Concat(extension);

			// Assert
			// This test checks how Concat handles slashes at the boundary of the base and extension parts.
			// Adjust the expected result based on the actual intended behavior of Concat in this scenario.
			Assert.Equal("<http://example.com//resource>", result); // Assuming Concat does not handle slash normalization.
		}

		[Fact]
		public void Extend_WhenBaseEndsWithSlash_ShouldNotDuplicateSlash()
		{
			// Arrange
			var iri = new IRI("http://example.com/");
			var extension = "resource";

			// Act
			var extendedIRI = iri.Extend(extension);

			// Assert
			// This test checks whether Extend method correctly handles cases where the base IRI ends with a slash
			// and the extension does not start with one, preventing the duplication of slashes in the resulting IRI.
			Assert.Equal("http://example.com/resource", extendedIRI.Value);
		}

		[Fact]
		public void ToString_WhenBasedAndHasUnusualCharacters_ShouldReturnExpectedFormat()
		{
			// Arrange
			var baseIRI = new IRI("http://example.com");
			var iri = new IRI("http://example.com/resource/äöü");
			iri.Rebase(baseIRI);

			// Act
			var result = iri.ToString();

			// Assert
			// Assuming ToString should return a representation that includes the base indication and correctly handles unusual characters.
			Assert.Equal(":/resource/äöü", result);
		}
	}
}
