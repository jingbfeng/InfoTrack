using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InfotrackAPI.Helper
{
	public class Helper
	{
        public class TimeOnlyJsonConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    var timeString = reader.GetString();
                    if (TimeOnly.TryParse(timeString, out var timeOnly))
                    {
                        return DateTime.Today.Add(timeOnly.ToTimeSpan());
                    }
                }
                throw new JsonException("Invalid time format.");
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("HH:mm"));
            }
        }
    }
}

