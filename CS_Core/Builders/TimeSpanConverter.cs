using System.Text.Json.Serialization;
using System.Text.Json;
namespace CS_Core
{
    /// <summary>
    /// TimespanConverter
    /// </summary>
    public sealed class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String) throw new JsonException();

            string value = reader.GetString() ?? string.Empty;

            if (!TimeSpan.TryParseExact(value, "hh\\:mm\\:ss", null, out TimeSpan result))
                throw new JsonException($"Invalid TimeSpan format: {value}");

            return result;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("hh\\:mm\\:ss"));
        }
    }
}
