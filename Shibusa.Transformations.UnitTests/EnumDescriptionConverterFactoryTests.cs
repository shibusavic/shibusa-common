using System;
using System.ComponentModel;
using System.Text.Json;
using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class JsonDescriptionEnumConverterTests
    {
        [Fact]
        public void Serialize()
        {
            TestClass test = new()
            {
                Name = nameof(Serialize),
                EnumerationWithDescription = EnumWithDescription.One,
                EnumerationWithoutDescription = EnumWithoutDescription.Two
            };

            string expected = "{\"Name\":\"Serialize\",\"EnumerationWithDescription\":\"First\",\"EnumerationWithoutDescription\":\"Two\"}";

            JsonSerializerOptions options = new();
            options.Converters.Add(new EnumDescriptionConverterFactory());

            string json = JsonSerializer.Serialize(test, options);

            Assert.NotNull(json);

            Assert.Equal(expected, json);
        }

        [Fact]
        public void Deserialize()
        {
            TestClass test = new()
            {
                Name = nameof(Deserialize),
                EnumerationWithDescription = EnumWithDescription.One,
                EnumerationWithoutDescription = EnumWithoutDescription.Two
            };
            string expected = "{\"Name\":\"Deserialize\",\"EnumerationWithDescription\":\"First\",\"EnumerationWithoutDescription\":\"Two\"}";
            JsonSerializerOptions options = new();
            options.Converters.Add(new EnumDescriptionConverterFactory());

            string json = JsonSerializer.Serialize(test, options);

            Assert.NotNull(json);

            Assert.Equal(expected, json);

            TestClass deserialized = JsonSerializer.Deserialize<TestClass>(json, options);

            Assert.Equal(test, deserialized);
        }

        [Fact]
        public void Deserialize_MemberNameUsed_Deserializes()
        {
            TestClass test = new()
            {
                Name = nameof(Deserialize),
                EnumerationWithDescription = EnumWithDescription.One,
                EnumerationWithoutDescription = EnumWithoutDescription.Two
            };
            string json = "{\"Name\":\"Deserialize\",\"EnumerationWithDescription\":\"One\",\"EnumerationWithoutDescription\":\"Two\"}";
            JsonSerializerOptions options = new();
            options.Converters.Add(new EnumDescriptionConverterFactory());

            TestClass deserialized = JsonSerializer.Deserialize<TestClass>(json, options);

            Assert.Equal(test, deserialized);
        }

        [Fact]
        public void Deserialize_ValueUsed_Deserializes()
        {
            TestClass test = new()
            {
                Name = nameof(Deserialize),
                EnumerationWithDescription = EnumWithDescription.One,
                EnumerationWithoutDescription = EnumWithoutDescription.Two
            };
            string json = "{\"Name\":\"Deserialize\",\"EnumerationWithDescription\":\"1\",\"EnumerationWithoutDescription\":\"2\"}";
            JsonSerializerOptions options = new();
            options.Converters.Add(new EnumDescriptionConverterFactory());

            TestClass deserialized = JsonSerializer.Deserialize<TestClass>(json, options);

            Assert.Equal(test, deserialized);
        }

        [Fact]
        public void Deserialize_BadValue_Throws()
        {
            TestClass test = new()
            {
                Name = nameof(Deserialize),
                EnumerationWithDescription = EnumWithDescription.One,
                EnumerationWithoutDescription = EnumWithoutDescription.Two
            };
            string json = "{\"Name\":\"Deserialize\",\"EnumerationWithDescription\":\"First\",\"EnumerationWithoutDescription\":\"Four\"}";

            JsonSerializerOptions options = new();
            options.Converters.Add(new EnumDescriptionConverterFactory());

            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestClass>(json, options));
        }
    }

    internal class TestClass : IEquatable<TestClass>
    {
        public string Name { get; set; }
        public EnumWithDescription EnumerationWithDescription { get; set; }
        public EnumWithoutDescription EnumerationWithoutDescription { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TestClass);
        }

        public bool Equals(TestClass other)
        {
            return other != null &&
                   Name == other.Name &&
                   EnumerationWithDescription == other.EnumerationWithDescription &&
                   EnumerationWithoutDescription == other.EnumerationWithoutDescription;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, EnumerationWithDescription, EnumerationWithoutDescription);
        }
    }

    internal enum EnumWithDescription
    {
        [Description("First")]
        One = 1,
        [Description("Second")]
        Two = 2,
        [Description("Third")]
        Three = 3
    }

    internal enum EnumWithoutDescription
    {
        One = 1,
        Two = 2,
        Three = 3
    }
}