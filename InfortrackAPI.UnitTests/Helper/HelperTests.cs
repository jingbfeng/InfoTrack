using System;
using System.Text.Json;
using InfotrackAPI.Helper;
using System.Text.Json.Serialization;
using NUnit.Framework;

namespace InfortrackAPI.UnitTests.Helper
{
    public class HelperTests
    {
        private InfotrackAPI.Helper.Helper.TimeOnlyJsonConverter _converter;
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            _converter = new InfotrackAPI.Helper.Helper.TimeOnlyJsonConverter();
            _options = new JsonSerializerOptions();
            _options.Converters.Add(_converter);
        }

        [Test]
        public void Read_ValidTimeString_ReturnsCorrectDateTime()
        {
            // Arrange
            var json = "\"15:30\"";
            var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            reader.Read();
            var result = _converter.Read(ref reader, typeof(DateTime), _options);

            // Assert
            var expected = DateTime.Today.Add(new TimeSpan(15, 30, 0));
            Assert.AreEqual(expected, result);
        }

        //[Test]
        //public void Read_InvalidTimeString_ThrowsJsonException()
        //{
        //    // Arrange
        //    var json = "\"invalid time\"";
        //    var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json), isFinalBlock: true, state: default);

        //    // Act & Assert
        //    Assert.Throws<JsonException>(() => PerformOperation(ref reader));
        //}

        //private void PerformOperation(ref Utf8JsonReader reader)
        //{
        //    ReadInvalidTimeString(ref reader);
        //}

        //private void ReadInvalidTimeString(ref Utf8JsonReader reader)
        //{
        //    _converter.Read(ref reader, typeof(DateTime), _options);
        //}

        [Test]
        public void Write_ValidDateTime_WritesCorrectTimeString()
        {
            // Arrange
            var dateTime = DateTime.Today.Add(new TimeSpan(15, 30, 0));
            var buffer = new System.Buffers.ArrayBufferWriter<byte>();
            var writer = new Utf8JsonWriter(buffer);

            // Act
            _converter.Write(writer, dateTime, _options);
            writer.Flush();
            var json = System.Text.Encoding.UTF8.GetString(buffer.WrittenSpan);

            // Assert
            Assert.AreEqual("\"15:30\"", json);
        }
    }
}

